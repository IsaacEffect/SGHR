namespace SGHR.Application.Exceptions
{
    internal class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException() : base("One or more validation errors occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }
        
        public ValidationException(IEnumerable<string> failures) : this()
        {
            Errors = failures.GroupBy(f => f.Contains(':') ? f.Split(':')[0].Trim() : "General")
                             .ToDictionary(
                                    g => g.Key,     
                                    g => g.Select(f => f.Contains(':') ? f.Split(':')[1].Trim() : f).ToArray());
        }
    }
}
