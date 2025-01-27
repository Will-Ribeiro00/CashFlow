using AutoMapper;
using CashFlow.Communication.Requests.RequestExpenseJson;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Update
{
    public class UpdateExpenseUseCase : IUpdateExpenseUseCase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExpensesUpdateOnlyRepository _repository;
        public UpdateExpenseUseCase(IMapper mapper, IUnitOfWork unitOfWork, IExpensesUpdateOnlyRepository repository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task Execute(int id, RequestExpenseJson request)
        {
            var expense = await _repository.GetById(id) ?? throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

            Validate(request);

            _mapper.Map(request, expense);
            _repository.Update(expense);

            await _unitOfWork.Commit();
        }

        private void Validate(RequestExpenseJson request)
        {
            var validator = new ExpenseValidator();
            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                var errorMessage = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessage);
            }
        }
    }
}
