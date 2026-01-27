using BuildingBlocks.Exceptions.Common;

namespace BuildingBlocks.Exceptions
{
    public class ConflictException : BaseException
    {
        public ConflictException(string message) : base(message, "Conflict")
        {
            
        }
    }
}
