using Promact.Trappist.DomainModel.Enum;
using System.ComponentModel.DataAnnotations;

namespace Promact.Trappist.DomainModel.ApplicationClasses.Question
{
    public class QuestionDetailAC
    {
        public int Id { get; set; }

        [Required]
        public string QuestionDetail { get; set; }

        [Required]
        public QuestionType QuestionType { get; set; }

        [Required]
        public DifficultyLevel DifficultyLevel { get; set; }

        public int CategoryID { get; set; }
    }
}
