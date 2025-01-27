using CashFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.Data_Acess
{
    internal class CashFlowDbContext : DbContext
    {
        public CashFlowDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Expense> Expenses { get; set; }

    }
}
