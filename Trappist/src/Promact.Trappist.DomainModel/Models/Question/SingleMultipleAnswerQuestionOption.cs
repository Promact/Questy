using System;
using System.ComponentModel.DataAnnotations;
namespace Promact.Trappist.DomainModel.Models.Question
{
    public class SingleMultipleAnswerQuestionOption
    {
        public int Id { get; set; }

        [Required]
        public string Option { get; set; }

        [Required]
        public bool IsAnswer { get; set; }

        [Required]
        public DateTime CreatedDateTime { get; set; }

        public DateTime? UpdateDateTime { get; set; }

        public int SingleMultipleAnswerQuestionID { get; set; }

        public virtual SingleMultipleAnswerQuestion SingleMultipleAnswerQuestion { get; set; }
   }
}
