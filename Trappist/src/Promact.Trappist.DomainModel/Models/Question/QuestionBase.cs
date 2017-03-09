using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using static Promact.Trappist.DomainModel.Models.Enums.EnumList;

namespace Promact.Trappist.DomainModel.Models.Question
{
    public class QuestionBase:BaseModel
    {
        [Required]
        public string QuestionDetail { get; set; }
        [Required]
        public QuestionType QuestionType { get; set; }
        [Required]
        public DifficultyLevel DifficultyLevel { get; set; }
        [Required]
        public int CreateBy { get; set; }
        public string UpdatedBy { get; set; }
        [Required]
        public int CategoryID { get; set; }

    }
}
