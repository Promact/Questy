using Promact.Trappist.DomainModel.Enum;
using System.Collections.Generic;

namespace Promact.Trappist.DomainModel.ApplicationClasses.Question
{
    /// <summary>
    ///Application class for code snippet question 
    /// </summary>
    public class CodeSnippetQuestionModel
    {
        public QuestionType QuestionType { get; set;}

        public DifficultyLevel  DifficultyLevel { get; set; }

        public string QuestionDetail { get; set; }

        public ICollection<CodingLanguageModel> ProgramingLanguage { get; set; }

        public bool CheckCodeComplexity { get; set; }

        public bool CheckTimeComplexity { get; set; }

        public bool RunBasicTestCase { get; set; }

        public bool RunCornerTestCase { get; set; }

        public bool RunNecessaryTestCase { get; set; }
    }
}
