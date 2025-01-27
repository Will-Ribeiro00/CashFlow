using System.Net;

namespace CashFlow.Exception.ExceptionsBase
{
    public class ErrorOnValidationException : CashFlowException
    {
        private readonly List<string> _errors;
        public override int StatusCode => (int)HttpStatusCode.BadRequest;

        public ErrorOnValidationException(List<string> ErrorMessagens) : base(string.Empty)
        {
            _errors = ErrorMessagens;
        }

        public override List<string> GetErrors()
        {
            return _errors;
        }
    }
}
