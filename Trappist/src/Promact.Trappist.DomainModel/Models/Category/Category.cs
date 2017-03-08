using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Trappist.DomainModel.Models.Catagory
{
    public class Category :BaseModel
    {
        #region Category Table
        /// <summary>
        /// catagory Model 
        /// Maxlength=150
        /// </summary>
        [Required]
        [MaxLength(150)]
        public string CategoryName { get; set; }
        #endregion
    }
}
