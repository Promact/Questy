using System.ComponentModel.DataAnnotations;

namespace Promact.Trappist.DomainModel.ApplicationClasses.Question
{
    public class SingleMultipleAnswerQuestionOptionAC
    {
        [Required]
        public string Option { get; set; }

        [Required]
        public bool IsAnswer { get; set; }
    }
}
