using System.Collections.Generic;

namespace Promact.Trappist.DomainModel.ApplicationClasses.Question
{
    public class CategoryAC
    {
        public string CategoryName { get; set; }
        public virtual ICollection<Models.Question.Question> Question { get; set; }                          
        public int Id { get; set; }
        public bool IsSelect { get; set; }
    }
}
