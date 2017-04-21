
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using System.Collections.Generic;

namespace Promact.Trappist.DomainModel.ApplicationClasses.Test
{
    public class CategoryAC
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public List<QuestionAC> QuestionList { get; set; }
        public bool IsSelect { get; set; }
    }
}
