using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
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
        [HttpGet("question")]
        public IActionResult GetQuestions()
        {
            var questions = _questionsRepository.GetAllQuestions();

            return Json(questions);
        }

        [HttpPost("codeSnippetQuestion")]
        public IActionResult AddCodeSnippetQuestion([FromBody]CodeSnippetQuestionModel codeSnippetQuestionModel)
        {
            if(codeSnippetQuestionModel == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            _questionsRepository.AddCodeSnippetQuestion(codeSnippetQuestionModel);
            
            return Ok(codeSnippetQuestionModel);

        }
    }
}
