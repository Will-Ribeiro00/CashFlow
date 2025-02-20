namespace CashFlow.Communication.Requests.RequestUserJson
{
    public class RequestChangePasswordJson
    {
        public string Password { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
