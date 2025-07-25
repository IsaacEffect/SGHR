namespace SGHR.Application.Exceptions
{
    internal class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }
}
