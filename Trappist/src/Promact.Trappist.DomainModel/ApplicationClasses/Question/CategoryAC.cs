using System.Collections.Generic;

namespace Promact.Trappist.DomainModel.ApplicationClasses.Question
{
    public class CategoryAC
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public List<QuestionAC> QuestionList { get; set; }
        public bool IsSelect = false;
    }
}
