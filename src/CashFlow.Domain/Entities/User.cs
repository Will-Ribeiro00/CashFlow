using CashFlow.Domain.Enums;
using System.Data;

namespace CashFlow.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }= string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Guid UserIdentifier { get; set; }
        public string Role { get; set; } = Roles.TEAM_MEMBER;
    }
}
