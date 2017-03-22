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
        
        public int CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Category.Category Category { get; set; }

    }
}
