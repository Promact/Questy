using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Trappist.DomainModel.Models
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; } 
        [Required]
        public DateTime CreatedDateTime { get; set; }   
        public DateTime? UpdateDateTime { get; set; }
    }
}
