using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.Repository.Questions;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api")]
    public class QuestionsController : Controller
    {
        private readonly IQuestionRepository _questionsRepository;

        public QuestionsController(IQuestionRepository questionsRepository)
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
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest();
            //}
            //else
            //{
            //    int _questionsRepository.AddCodeSnippetQuestion(codeSnippetQuestion);
            //}
            return Ok();

        }
    }
}
