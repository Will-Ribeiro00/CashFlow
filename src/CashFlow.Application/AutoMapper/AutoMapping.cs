using AutoMapper;
using CashFlow.Communication.Requests.RequestExpenseJson;
using CashFlow.Communication.Responses.ResponseExpenseJson;
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
            CreateMap<RequestExpenseJson, Expense>();
        }
        private void EntityToResponse()
        {
            CreateMap<Expense, ResponseRegisterExpenseJson>();
            CreateMap<Expense, ResponseShortExpenseJson>();
            CreateMap<Expense, ResponseExpenseJson>();
        }
    }
}
