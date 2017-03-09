using System.ComponentModel.DataAnnotations;
namespace Promact.Trappist.DomainModel.Models.Category
{
    public class Category : BaseModel
    {
        [Required]
        [MaxLength(150)]
        public string categoryName { get; set; }
    }
}
