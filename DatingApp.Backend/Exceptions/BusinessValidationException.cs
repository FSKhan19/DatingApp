namespace DatingApp.Backend.Exceptions
{
    public class BusinessValidationException : Exception
    {
        public BusinessValidationException() { }

        public BusinessValidationException(string message) : base(message)
        {
            Data[ExceptionType.Business.ToString()] = message;
        }

        public BusinessValidationException(string message,
                                           Exception innerException)
            : base(message, innerException)
        {
            Data[ExceptionType.Business.ToString()] = message;
        }
        public BusinessValidationException(string[] exceptions) : base(exceptions.Count() > 0 ? exceptions[0] : "Empty Array")
        {
            Data[ExceptionType.Business.ToString()] = exceptions;
        }

        public BusinessValidationException(string[] exceptions, Exception innerException)
            : base(exceptions.Count() > 0 ? exceptions[0] : "Empty Array", innerException)
        {
            Data[ExceptionType.Business.ToString()] = exceptions;
        }
    }
}
