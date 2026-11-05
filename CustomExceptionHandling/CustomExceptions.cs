namespace ConfidentVotesAPI.CustomExceptionHandling
{   
    // base exception
    public abstract class ConfidentAPIException : Exception
    {
        public int StatusCode { get; }
        public string ErrorCode { get; }

        protected ConfidentAPIException(
            string message,
            int statusCode,
            string errorCode)
            : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }
    }
    public sealed class BadHeaderException : ConfidentAPIException
    {
        public BadHeaderException(string headerName)
            : base(
                $"Invalid or missing header: {headerName}.",
                StatusCodes.Status400BadRequest,
                "Inconsistent header entry")
        {
        }
    }
    // body ill-formatted
    public sealed class BadBodyException : ConfidentAPIException
    {
        public BadBodyException(string message)
        :base(
                message,
                StatusCodes.Status400BadRequest,
                "Body not well-formatted"
            )
        {
            
        }
        
    }
     public sealed class BadPathException : ConfidentAPIException
    {
        public BadPathException(string path)
        :base(
                $"{path} Is not valid",
                StatusCodes.Status404NotFound,
                "Path not retrievable"
            )
        {
            
        }
        
    }
    public sealed class BadKeyException : ConfidentAPIException
    {
        public BadKeyException(string key)
            : base(
                $"The key '{key}' is not allowed.",
                StatusCodes.Status400BadRequest,
                "BAD_KEY")
        {
        }
    }
    // field missing or empty in body
    public sealed class BadValueException : ConfidentAPIException
    {
        public BadValueException (string field)
            : base(
                $"The value for '{field}' is invalid.",
                StatusCodes.Status400BadRequest,
                "bad entry in body")
        {
            
        }
    }
    public sealed class MissingFieldException : ConfidentAPIException
    {
        public MissingFieldException(string field)
            : base(
                $"The required field '{field}' is missing.",
                StatusCodes.Status400BadRequest,
                "MISSING_FIELD")
        {
        }
    }

 }