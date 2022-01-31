namespace Promact.Trappist.DomainModel.ApplicationClasses.CodeSnippet
{
    public class Result
    {
        public string Output { get; set; }
        //In seconds
        public long RunTime { get; set; }

        public int CyclicMetrics { get; set; }

        public int ExitCode { get; set; }

        public long MemoryConsumed { get; set; }
    }
}