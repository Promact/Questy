using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promact.Trappist.DomainModel.Models.Test
{
    public class TestQuestion
    {
        [Key]
        public int Id { get; set; }    
        public int QuestionId { get; set; }
        public int TestCategoryId { get; set; }

        [ForeignKey("QuestionId")]
        public virtual Question.Question Question { get; set; }

        [ForeignKey("TestCategoryId")]
        public virtual TestCategory TestCategory { get; set; }
    }
}
