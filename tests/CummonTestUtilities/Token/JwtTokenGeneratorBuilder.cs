using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Tokens;
using Moq;

namespace CummonTestUtilities.Token
{
    public class JwtTokenGeneratorBuilder
    {
        public static IAccessTokenGenerator Builder()
        {
            var mock = new Mock<IAccessTokenGenerator>();
            mock.Setup(accessTokenGenerator => accessTokenGenerator.Generate(It.IsAny<User>())).Returns("\"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IlRlc3RlIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvc2lkIjoiNmIyYzYwYzUtMjM3NC00YTcyLWFlOWEtMjZlYTdlMzcwNGRmIiwibmJmIjoxNzM4MDk3NDg1LCJleHAiOjE3MzgxNTc0ODUsImlhdCI6MTczODA5NzQ4NX0.jdX82x_5qyuLm9O-WfiuY9dEvt0LqQwli30_rOJy-5A\"");

            return mock.Object;
        }
    }
}
