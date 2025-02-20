using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Delete
{
    public class DeleteExpenseUseCase : IDeleteExpenseUseCase
    {
        private readonly IExpensesWriteOnlyRepository _writeOnlyRepository;
        private readonly IExpensesReadOnlyRepository _readOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggedUser _loggedUser;
        public DeleteExpenseUseCase(IExpensesWriteOnlyRepository writeOnlyRepository, IExpensesReadOnlyRepository readOnlyRepository, ILoggedUser loggedUser, IUnitOfWork unitOfWork)
        {
            _writeOnlyRepository = writeOnlyRepository;
            _readOnlyRepository = readOnlyRepository;
            _loggedUser = loggedUser;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(int id)
        {
            var loggedUser = await _loggedUser.Get();
            var expense = await _readOnlyRepository.GetById(id, loggedUser);

            if (expense is null) throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

            await _writeOnlyRepository.Delete(id);

            await _unitOfWork.Commit();
        }
    }
}
