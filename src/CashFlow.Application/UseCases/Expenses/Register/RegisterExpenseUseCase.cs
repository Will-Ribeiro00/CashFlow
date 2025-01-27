using AutoMapper;
using CashFlow.Communication.Requests.RequestExpenseJson;
using CashFlow.Communication.Responses.ResponseExpenseJson;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Register
{
    public class RegisterExpenseUseCase : IRegisterExpenseUseCase
    {
        private readonly IExpensesWriteOnlyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RegisterExpenseUseCase(IExpensesWriteOnlyRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseRegisterExpenseJson> Execute(RequestExpenseJson request)
        {
            Validade(request);

            var entity = _mapper.Map<Expense>(request);

            await _repository.Add(entity);

            await _unitOfWork.Commit();

            return _mapper.Map<ResponseRegisterExpenseJson>(entity);
        }

        private void Validade(RequestExpenseJson request)
        {
            var validator = new ExpenseValidator();

            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                var erroMessagens = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(erroMessagens);
            }
        }
    }
}
