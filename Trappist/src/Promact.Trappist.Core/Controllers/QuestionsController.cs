using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.Repository.Questions;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api")]
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
        [HttpGet("Questions")]
        public IActionResult GetQuestions()
        {
            var questions = _questionsRepository.GetAllQuestions();

            return Json(questions);
        }

        [HttpPost("CodeSnippetQuestion")]
        public IActionResult PostCodeSnippetQuestion([FromBody]CodeSnippetQuestion codeSnippetQuestion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                int response = _questionsRepository.AddCodeSnippetQuestion(codeSnippetQuestion);

            }
            return Ok();

        }
    }
}
