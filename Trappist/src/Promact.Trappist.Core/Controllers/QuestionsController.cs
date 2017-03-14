using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.Repository.Questions;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api/Questions")]
    public class QuestionsController : Controller
    {
        private readonly IQuestionsRepository _questionsRepository;

        public QuestionsController(IQuestionsRepository questionsRepository)
        {
            _questionsRepository = questionsRepository;
        }

        /// <summary>
        /// Gets all questions
        /// </summary>
        /// <returns>Questions list</returns>
        [HttpGet]
        public IActionResult GetQuestions()
        {
            var questions = _questionsRepository.GetAllQuestions();

            return Json(questions);
        }
    }
}
