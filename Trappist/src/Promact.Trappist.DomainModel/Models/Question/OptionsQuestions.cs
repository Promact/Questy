using System.ComponentModel.DataAnnotations.Schema;

namespace Promact.Trappist.DomainModel.Models.Question
{
    public class OptionsQuestions:BaseModel
    {
        public string optionDetails { get; set; }
        public int QuestionID;
        [ForeignKey("QuestionId")]
        public Question Question { get; set; }

    }
}
