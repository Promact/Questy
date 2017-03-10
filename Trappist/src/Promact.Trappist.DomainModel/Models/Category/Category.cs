﻿using System.Collections.Generic;
using Promact.Trappist.DomainModel.Models.Question;
using System.ComponentModel.DataAnnotations;
using Promact.Trappist.DomainModel.Models.Question;

namespace Promact.Trappist.DomainModel.Models.Category
{
    public class Category : BaseModel
    {
        [Required]
        [MaxLength(50)]
        public string CategoryName { get; set; }
        public virtual ICollection<Question> questions { get; set; }
        [Required]
        public virtual ICollection<SingleMultipleAnswerQuestion> SingleMultipleAnswerQuestion { get; set; }
    }
}
