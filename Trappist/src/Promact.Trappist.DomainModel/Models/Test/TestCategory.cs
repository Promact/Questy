using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promact.Trappist.DomainModel.Models.Test
{
    public class TestCategory
    {
        [Key]
        int? Id { get; set; }
        int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category.Category Category {get;set;}
        int TestId { get; set; }
        [ForeignKey("TestId")]
        public virtual Test Test { get; set; }
    }
}
