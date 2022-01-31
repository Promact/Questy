using Promact.Trappist.DomainModel.Enum;

namespace Promact.Trappist.DomainModel.ApplicationClasses.CodeSnippet
{
    public class Code
    {
        public string Key { get; set; }

        public string Source { get; set; }

        public string Input { get; set; }

        public ProgrammingLanguage Language { get; set; }

        public CodeResponse CodeResponse { get; set; }
    }
}