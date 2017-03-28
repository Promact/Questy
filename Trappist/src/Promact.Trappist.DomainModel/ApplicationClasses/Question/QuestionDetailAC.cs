using Promact.Trappist.DomainModel.Enum;

namespace Promact.Trappist.DomainModel.ApplicationClasses.Question
{
    public class QuestionDetailAC
    {
        public string QuestionDetail { get; set; }

        public QuestionType QuestionType { get; set; }

        public DifficultyLevel DifficultyLevel { get; set; }

        public int CategoryID { get; set; }
    }
}
