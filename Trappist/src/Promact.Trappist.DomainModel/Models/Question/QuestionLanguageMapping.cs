using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Trappist.DomainModel.Models.Question
{
    public class QuestionLanguageMapping: BaseModel
    {
        [Required]
        public int QuestionId { get; set; }

        [Required]
        public int LanguageId { get; set; }

        [ForeignKey("QuestionId")]
        public CodeSnippetQuestion codeSnippetQuestion { get; set; }

        [ForeignKey("LanguageId")]
        public CodingLanguage codeLanguage { get; set; }
    }
}
