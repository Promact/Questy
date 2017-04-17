using Promact.Trappist.DomainModel.Models.Question;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Promact.Trappist.DomainModel.ApplicationClasses.Question
{
    public class SingleMultipleAnswerQuestionOptionAC
    {
        [Required]
        public string Option { get; set; }

        [Required]
        public bool IsAnswer { get; set; }

        public int SingleMultipleAnswerQuestionID { get; set; }

        public virtual SingleMultipleAnswerQuestion SingleMultipleAnswerQuestion { get; set; }
    }
}
