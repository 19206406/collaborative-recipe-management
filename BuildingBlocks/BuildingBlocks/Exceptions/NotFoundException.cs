using BuildingBlocks.Exceptions.Common;

namespace BuildingBlocks.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string name, object key)
            : base($"{name} con identificador {key} no fue encontrado.", "NotFound")
        {
            
        }
    }
}
