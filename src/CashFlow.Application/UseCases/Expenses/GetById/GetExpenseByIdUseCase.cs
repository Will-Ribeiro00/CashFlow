using AutoMapper;
using CashFlow.Communication.Responses.ResponseExpenseJson;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.GetById
{
    public class GetExpenseByIdUseCase : IGetExpenseByIdUseCase
    {
        private readonly IExpensesReadOnlyRepository _repositoy;
        private readonly IMapper _mapper;
        public GetExpenseByIdUseCase(IExpensesReadOnlyRepository repositoy, IMapper mapper)
        {
            _repositoy = repositoy;
            _mapper = mapper;
        }

        public async Task<ResponseExpenseJson> Execute(int id)
        {
            var result = await _repositoy.GetById(id);

            if(result is null)
            {
                throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
            }

            return _mapper.Map<ResponseExpenseJson>(result);
        }
    }
}
