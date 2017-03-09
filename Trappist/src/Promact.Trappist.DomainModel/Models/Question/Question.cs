using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Promact.Trappist.DomainModel.Models.Question
{
    public class Question : BaseQuestion
    {
        [Required]
        public int CategoryID { get; set; }
        [Required]
        public ICollection<Options> Options { get; set; }

    }
}
