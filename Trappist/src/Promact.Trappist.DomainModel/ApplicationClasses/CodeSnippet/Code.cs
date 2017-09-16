using Promact.Trappist.DomainModel.ApplicationClasses.CodeSnippet;
using Promact.Trappist.DomainModel.Enum;

namespace CodeBaseSimulator.Models
{
    public class Code
    {
        public string Key;

        public string Source;

        public string Input;

        public ProgrammingLanguage Language;

        public CodeResponse CodeResponse;
    }
}