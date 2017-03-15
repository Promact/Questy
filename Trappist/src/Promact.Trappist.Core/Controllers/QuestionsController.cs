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

        /// <summary>
        /// Adds new code snippet question to the database
        /// </summary>
        /// <param name="codeSnippetQuestionModel">Code snippet question model</param>
        /// <returns></returns>
        [HttpPost("codeSnippetQuestion")]
        public IActionResult AddCodeSnippetQuestion([FromBody]CodeSnippetQuestionDto codeSnippetQuestionModel)
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
