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
        public CodeSnippetQuestion CodeSnippetQuestion { get; set; }

        [ForeignKey("LanguageId")]
        public CodingLanguage CodeLanguage { get; set; }
    }
}
