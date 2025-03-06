using BC = BCrypt.Net.BCrypt;
using CashFlow.Domain.Security.Cryptography;

namespace CashFlow.Infrastructure.Security.Cryptography
{
    internal class BCrypt : IPasswordEncrypter
    {
        public string Encrypt(string password) => BC.HashPassword(password);
        public bool Verify(string password, string passwordHash) => BC.Verify(password, passwordHash);
    }
}
