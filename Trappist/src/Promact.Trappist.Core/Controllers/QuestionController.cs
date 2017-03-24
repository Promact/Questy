using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.Repository.Questions;
using System.Threading.Tasks;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api/question")]
    public class QuestionController : Controller
    {
        private readonly IQuestionRespository _questionsRepository;

        public QuestionController(IQuestionRespository questionsRepository)
        {
            _questionsRepository = questionsRepository;
        }

        #region Question API
        /// <summary>
        /// Post API to save the question
        /// </summary>
        /// <param name="questionAC">QuestionAC object</param>
        /// <returns>
        /// Returns added question
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> AddQuestion([FromBody]QuestionAC questionAC)
        {
            if (questionAC == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            if (questionAC.Question.QuestionType == QuestionType.Programming)
            {
                //To-Do Modify parameter accordingly _questionsRepository.AddCodeSnippetQuestion(questionAC);
            }
            else
            {
                await _questionsRepository.AddSingleMultipleAnswerQuestionAsync(questionAC);
            }
            return Ok(questionAC);
        }

        /// <summary>
        /// Returns The List Of Questions
        /// </summary>
        /// <returns></returns>
        [HttpGet("question")]
        public IActionResult GetAllQuestions()
        {
            var questionsList = _questionsRepository.GetAllQuestions();
            return Ok(questionsList);
        }
        #endregion
    }
}
