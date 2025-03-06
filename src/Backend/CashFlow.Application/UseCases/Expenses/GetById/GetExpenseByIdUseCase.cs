using AutoMapper;
using CashFlow.Communication.Responses.ResponseExpenseJson;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.GetById
{
    public class GetExpenseByIdUseCase : IGetExpenseByIdUseCase
    {
        private readonly IExpensesReadOnlyRepository _repositoy;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;
        public GetExpenseByIdUseCase(IExpensesReadOnlyRepository repositoy, IMapper mapper, ILoggedUser loggedUser)
        {
            _repositoy = repositoy;
            _mapper = mapper;
            _loggedUser = loggedUser;
        }

        public async Task<ResponseExpenseJson> Execute(int id)
        {
            var loggedUser = await _loggedUser.Get();

            var result = await _repositoy.GetById(id, loggedUser);

            if(result is null)
            {
                throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
            }

            return _mapper.Map<ResponseExpenseJson>(result);
        }
    }
}
