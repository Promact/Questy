using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promact.Trappist.DomainModel.Models.Test
{
    public class TestCategory
    {
        [Key]
        public int Id { get; set; }
        public int TestId { get; set; }
        [ForeignKey("TestId")]
        public Test Test { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category.Category Category { get; set; }
    }
}
