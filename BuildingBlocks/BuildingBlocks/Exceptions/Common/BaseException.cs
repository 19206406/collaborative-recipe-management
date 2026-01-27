namespace BuildingBlocks.Exceptions.Common
{
    public class BaseException : Exception
    {
        public string Code { get; }

        public BaseException(string message, string code) : base(message)
        {
            Code = code; 
        }
    }
}
