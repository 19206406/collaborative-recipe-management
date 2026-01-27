using BuildingBlocks.Exceptions.Common;

namespace BuildingBlocks.Exceptions
{
    public class ForbiddenException : BaseException
    {
        public ForbiddenException(string message = "Acceso prohibido")
            : base(message, "Forbidden")
        {
            
        }
    }
}
