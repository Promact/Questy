using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.Repository.Questions;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api/Questions")]
    public class QuestionsController : Controller
    {
        private readonly IQuestionsRespository _questionsRepository;

        public QuestionsController(IQuestionsRespository questionsRepository)
        {
            _questionsRepository = questionsRepository;
        }

        /// <summary>
        /// Gets all questions
        /// </summary>
        /// <returns>Questions list</returns>
        [HttpGet("all")]
        public IActionResult GetAllQuestions()
        {
            var questions = _questionsRepository.GetAllQuestions();
            return Json(questions);

        }
		
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var categoryList = _questionsRepository.GetAllCategories();

            return Json(categoryList);
        }
    }
}
