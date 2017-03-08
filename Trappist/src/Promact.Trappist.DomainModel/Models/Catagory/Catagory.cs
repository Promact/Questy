using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Trappist.DomainModel.Models.Catagory
{
    public class Catagory : BaseModel
    {
        [Required]
        [MaxLength(150)]
        public string CatagoryName { get; set; }
    }
}
