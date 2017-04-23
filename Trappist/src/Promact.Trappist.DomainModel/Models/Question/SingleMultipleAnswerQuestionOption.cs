using System.ComponentModel.DataAnnotations;
namespace Promact.Trappist.DomainModel.Models.Question
{
    public class SingleMultipleAnswerQuestionOption:BaseModel
    {
        [Required]
        public string Option { get; set; }
        [Required]
        public bool IsAnswer { get; set; }
        public int SingleMultipleAnswerQuestionID { get; set; }
        public virtual SingleMultipleAnswerQuestion SingleMultipleAnswerQuestion { get; set; }
   }
}
