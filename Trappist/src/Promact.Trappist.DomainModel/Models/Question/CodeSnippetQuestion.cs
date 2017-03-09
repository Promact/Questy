using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Trappist.DomainModel.Models.Question
{
    public class CodeSnippetQuestion : BaseModel
    {
   
        [Required]
        public bool CheckCodeComplexity { get; set; }

        [Required]
        public bool CheckTimeComplexity { get; set; }

        [Required]
        public bool RunBasicTestCase { get; set; }

        [Required]
        public bool RunCornerTestCase { get; set; }

        [Required]
        public bool RunNecessaryTestCase { get; set; }
    }
}
