using BuildingBlocks.Exceptions.Common;

namespace BuildingBlocks.Exceptions
{
    public class BusinessException : BaseException
    {
        public BusinessException(string message) : base(message, "BusinessRuleViolation")
        {
            
        }
    }
}
