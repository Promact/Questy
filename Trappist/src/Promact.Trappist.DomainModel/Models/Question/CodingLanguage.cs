using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Trappist.DomainModel.Models.Question
{
    public enum language: int
    {
        java,
        cpp,
        c
    }

    public class CodingLanguage: BaseModel
    { 
        [Required]
        public language Language { get; set; }

    }
}
