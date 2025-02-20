using CashFlow.Application.UseCases.Users.GetProfile;
using CashFlow.Domain.Entities;
using CummonTestUtilities.Entities;
using CummonTestUtilities.Mapper;
using CummonTestUtilities.Services;
using Shouldly;

namespace UseCase.Test.Users.Profile
{
    public class GetUserProfileUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            var user = UserBuilder.Build();
            var useCase = CreateUseCase(user);

            //Act
            var result = await useCase.Execute();

            //Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Name.ShouldBe(user.Name),
                () => result.Email.ShouldBe(user.Email)
                );
        }

        private GetUserProfileUseCase CreateUseCase(User user)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var mapper = MapperBuilder.Build();

            return new GetUserProfileUseCase(loggedUser, mapper);
        }
    }
}
