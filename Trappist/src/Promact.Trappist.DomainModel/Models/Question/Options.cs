using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Trappist.DomainModel.Models.Question
{
    public class Options:BaseModel
    {
        [Required]
        public string SingleMultipleAnswerOption { get; set; }
        public bool IsAnswer { get; set; }
        [ForeignKey("Question_Id")]
        public SingleMultipleAnswerQuestion Question { get; set; }

    }
}
