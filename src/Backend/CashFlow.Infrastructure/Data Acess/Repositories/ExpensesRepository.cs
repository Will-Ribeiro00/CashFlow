using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace CashFlow.Infrastructure.Data_Acess.Repositories
{
    internal class ExpensesRepository : IExpensesReadOnlyRepository, IExpensesWriteOnlyRepository, IExpensesUpdateOnlyRepository
    {
        private readonly CashFlowDbContext _context;
        public ExpensesRepository(CashFlowDbContext context) => _context = context;


        public async Task Add(Expense expense) => await _context.Expenses.AddAsync(expense);
        public async Task<List<Expense>> GetAll(User user) => await _context.Expenses.AsNoTracking().Where(expense => expense.UserId == user.Id).ToListAsync();
        async Task<Expense?> IExpensesReadOnlyRepository.GetById(int id, User user) 
        {
            return await GetFullExpense()
                .AsNoTracking()
                .FirstOrDefaultAsync(expense => expense.Id == id && expense.UserId == user.Id);
        }
        async Task<Expense?> IExpensesUpdateOnlyRepository.GetById(int id, User user)
        {
            return await GetFullExpense()
                .FirstOrDefaultAsync(expense => expense.Id == id && expense.UserId == user.Id);
        }
        public async Task Delete(int id)
        {
            var result = await _context.Expenses.FindAsync(id);

            _context.Expenses.Remove(result!);
        }
        public void Update(Expense expense) => _context.Expenses.Update(expense);

        public async Task<List<Expense>> FilterByMonth(DateOnly date, User user)
        {
            var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date;

            var daysInMounth = DateTime.DaysInMonth(year: date.Year, month: date.Month);
            var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMounth, hour: 23, minute: 59, second: 59);

            return await _context.Expenses.AsNoTracking()
                                          .Where(expense => expense.UserId == user.Id && expense.Date >= startDate && expense.Date <= endDate)
                                          .OrderBy(expense => expense.Date)
                                          .ThenBy(expense => expense.Title)
                                          .ToListAsync();
        }

        private IIncludableQueryable<Expense, ICollection<Tag>> GetFullExpense()
        {
            return _context.Expenses.Include(expense => expense.Tags);
        }
    }
}
