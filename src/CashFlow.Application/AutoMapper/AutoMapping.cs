using AutoMapper;
using CashFlow.Communication.Requests.RequestExpenseJson;
using CashFlow.Communication.Requests.RequestUserJson;
using CashFlow.Communication.Responses.ResponseExpenseJson;
using CashFlow.Communication.Responses.ResponseUserJson;
using CashFlow.Domain.Entities;

namespace CashFlow.Application.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            RequestToEntity();
            EntityToResponse();
        }

        private void RequestToEntity()
        {
            CreateMap<RequestRegisterUserJson, User>()
                .ForMember(destino => destino.Password, config => config.Ignore());

            CreateMap<RequestExpenseJson, Expense>()
                .ForMember(destino => destino.Tags, config => config.MapFrom(source => source.Tags.Distinct()));
            
            CreateMap<Communication.Enums.Tag, Tag>()
                .ForMember(destino => destino.Value, config => config.MapFrom(source => source));
        }
        private void EntityToResponse()
        {
            CreateMap<Expense, ResponseExpenseJson>()
                .ForMember(destino => destino.Tags, config => config.MapFrom(source => source.Tags.Select(tag => tag.Value)));

            CreateMap<Expense, ResponseRegisterExpenseJson>();
            CreateMap<Expense, ResponseShortExpenseJson>();
            CreateMap<User, ResponseUserProfileJson>();
        }
    }
}
