using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.Category;

namespace Promact.Trappist.DomainModel.ApplicationClasses.Question
{
    public class QuestionModelAC
    {
        public string QuestionDetail { get; set; }

        public QuestionType QuestionType { get; set; }

        public DifficultyLevel DifficultyLevel { get; set; }

        public Category Category { get; set; }
    }
}
