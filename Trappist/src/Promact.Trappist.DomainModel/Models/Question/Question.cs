using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promact.Trappist.DomainModel.Models.Question
{
    public class Question : BaseModel
    {
        public string Name { get; set; }
		
		 public int CategoryId;
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public virtual ICollection<OptionsQuestions> OptionProvided { get; set; }
    }
}
