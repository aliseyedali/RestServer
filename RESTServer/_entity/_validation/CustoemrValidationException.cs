namespace RESTServer
{
    public class CustoemrValidationException : Exception
    {
        public CustoemrValidationException(string message)
                : base(message) 
        { 
        }
    }
}
