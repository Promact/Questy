using Promact.Trappist.DomainModel.Enum;
using System.ComponentModel.DataAnnotations;


namespace Promact.Trappist.DomainModel.Models.Question
{
    public class QuestionBase:BaseModel
    {
        [Required]
        public string QuestionDetail { get; set; }
        [Required]
        public QuestionType QuestionType { get; set; }
        [Required]
        public DifficultyLevel DifficultyLevel { get; set; }
        [Required]
        public string CreateBy { get; set; }
        public string UpdatedBy { get; set; }
        [Required]
        public int CategoryID { get; set; }
        public virtual Category.Category Category { get; set; }
    }
}
