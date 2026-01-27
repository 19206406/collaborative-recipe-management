using BuildingBlocks.Exceptions.Common;

namespace BuildingBlocks.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message) : base(message, "BadRequest")
        {
            
        }
    }
}
