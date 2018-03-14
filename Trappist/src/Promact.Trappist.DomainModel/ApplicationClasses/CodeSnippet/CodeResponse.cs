namespace Promact.Trappist.DomainModel.ApplicationClasses.CodeSnippet
{
    public class CodeResponse
    {
        public string Message { get; set; }

        public string Error { get; set; }

        public bool ErrorOccurred { get; set; }

        public string Output { get; set; }

        public long TimeConsumed { get; set; }

        public long MemoryConsumed { get; set; }

        public int TotalTestCasePassed { get; set; }

        public int TotalTestCases { get; set; }
    }
}
