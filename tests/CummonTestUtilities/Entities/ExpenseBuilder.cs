using Bogus;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;

namespace CummonTestUtilities.Entities
{
    public class ExpenseBuilder
    {
        public static List<Expense> Collection(User user, uint count = 2)
        {
            var list = new List<Expense>();

            if (count == 0 ) 
                count = 1;

            var expenseId = 1;
            for(int i = 0; i < count; i++)
            {
                var expense = Build(user);
                expense.Id = expenseId++;

                list.Add(expense);
            }

            return list;
        }

        public static Expense Build(User user)
        {
            return new Faker<Expense>()
                .RuleFor(ex => ex.Id, _ => 1)
                .RuleFor(ex => ex.Title, faker => faker.Commerce.ProductName())
                .RuleFor(ex => ex.Description, faker => faker.Commerce.ProductDescription())
                .RuleFor(ex => ex.Date, faker => faker.Date.Past())
                .RuleFor(ex => ex.Amount, faker => faker.Random.Decimal(min: 1, max: 3000))
                .RuleFor(ex => ex.PaymentType, faker => faker.PickRandom<PaymentType>())
                .RuleFor(ex => ex.UserId, _ => user.Id)
                .RuleFor(ex => ex.Tags, faker => faker.Make(1, () => new CashFlow.Domain.Entities.Tag
                {
                    Id = 1,
                    Value = faker.PickRandom<CashFlow.Domain.Enums.Tag>(),
                    ExpenseId = 1
                }));
        }
    }
}
