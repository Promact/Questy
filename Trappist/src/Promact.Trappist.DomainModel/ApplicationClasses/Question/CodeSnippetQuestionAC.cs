using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.Question;
using System.Collections.Generic;

namespace Promact.Trappist.DomainModel.ApplicationClasses.Question
{
    public class CodeSnippetQuestionAC : CodeSnippetQuestion
    {
        public ICollection<ProgramingLanguage> LanguageList { get; set; }
    }
}
