using Bogus;
using CashFlow.Communication.Requests.RequestUserJson;

namespace CummonTestUtilities.Requests
{
    public class RequestChangePasswordJsonBuilder
    {
        public static RequestChangePasswordJson Build()
        {
            return new Faker<RequestChangePasswordJson>()
                .RuleFor(user => user.Password, faker => faker.Internet.Password())
                .RuleFor(user => user.NewPassword, faker => faker.Internet.Password(prefix: "!Aa1"));
        }
    }
}
