using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promact.Trappist.DomainModel.Models.Test
{
    public class TestQuestion
    {
        [Key]
        public int? Id { get; set; }
        public int TestId { get; set; }
        [ForeignKey("TestId")]
        public Test Test { get; set; }
        public int QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public Question.Question Question { get; set; }
        public int TestCategoryId { get; set; }
        [ForeignKey("TestCategoryId")]
        public TestCategory TestCategory { get; set; }
    }
}
