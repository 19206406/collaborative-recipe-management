using BuildingBlocks.Exceptions.Common;

namespace BuildingBlocks.Exceptions
{
    public class InvalidOperationException : BaseException
    {
        public InvalidOperationException(string message) : base(message, "InvalidOperation")
        {            
        }
    }
}
