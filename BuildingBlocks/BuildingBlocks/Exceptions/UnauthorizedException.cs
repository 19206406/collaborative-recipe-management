using BuildingBlocks.Exceptions.Common;

namespace BuildingBlocks.Exceptions
{
    public class UnauthorizedException : BaseException
    {
        public UnauthorizedException(string message = "No autorizado")
            : base(message, "Unauthorized")
        {
            
        }
    }
}
