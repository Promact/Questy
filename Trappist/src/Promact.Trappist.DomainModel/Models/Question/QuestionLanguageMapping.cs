using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promact.Trappist.DomainModel.Models.Question
{
    public class QuestionLanguageMapping: BaseModel
    {
        [Required]
        public int QuestionId { get; set; }

        [Required]
        public int LanguageId { get; set; }

        [ForeignKey("QuestionId")]
        public virtual CodeSnippetQuestion CodeSnippetQuestion { get; set; }

        [ForeignKey("LanguageId")]
        public virtual CodingLanguage CodeLanguage { get; set; }
    }
}
