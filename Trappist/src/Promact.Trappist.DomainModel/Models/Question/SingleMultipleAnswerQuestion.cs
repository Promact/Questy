using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promact.Trappist.DomainModel.Models.Question
{
    public class SingleMultipleAnswerQuestion
    {
        [ForeignKey("Question")]
        public int Id { get; set; }
        [Required]
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }

        public virtual ICollection<SingleMultipleAnswerQuestionOption> SingleMultipleAnswerQuestionOption { get; set; }

        public virtual Question Question { get; set; }
    }
}
