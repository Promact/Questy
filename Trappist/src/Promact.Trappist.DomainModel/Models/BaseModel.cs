using System;
using System.ComponentModel.DataAnnotations;


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
