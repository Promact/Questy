using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Promact.Trappist.DomainModel.Models.Question
{
    public class BaseQuestion:BaseModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string DifficultyLevel { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public string UpdatedBy { get; set; }
      

    }
}
