namespace CashFlow.Communication.Responses.ResponseErrorJson
{
    public class ResponseErrorJson
    {
        public List<string> ErrorMessages { get; set; } = [];

        public ResponseErrorJson(string erroMessage)
        {
            ErrorMessages = [erroMessage];
        }

        public ResponseErrorJson(List<string> erroMessages)
        {
            ErrorMessages = erroMessages;
        }
    }
}
