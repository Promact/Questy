using Promact.Trappist.DomainModel.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promact.Trappist.DomainModel.Models.Question
{
    public class Question:BaseModel
    {
        [Required]
        public string QuestionDetail { get; set; }

        [Required]
        public QuestionType QuestionType { get; set; }

        [Required]
        public DifficultyLevel DifficultyLevel { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public int CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Category.Category Category { get; set; }

        [ForeignKey("SingleMultipleAnswerQuestionId")]
        public int SingleMultipleAnswerQuestionId { get; set; }
        public virtual SingleMultipleAnswerQuestion SingleMultipleAnswerQuestion { get; set; }

        [ForeignKey(" CodeSnippetQuestionId")]
        public int CodeSnippetQuestionId { get; set; }
        public virtual CodeSnippetQuestion CodeSnippetQuestion { get; set; }
    }
}
