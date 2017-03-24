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
        /// Adds question to the database
        /// </summary>
        /// <param name="questionAC">QuestionAC object</param>
        /// <returns>
        /// Returns status 200(Ok) with QuestionAC object passed if question is added successfully
        /// Returns Status 400(BadRequest) if model state is invalid or null
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> AddQuestion([FromBody]QuestionAC questionAC)
        {
            if(questionAC == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            if(questionAC.SingleMultipleAnswerQuestionAC.SingleMultipleAnswerQuestion.Question.QuestionType == QuestionType.Single || questionAC.SingleMultipleAnswerQuestionAC.SingleMultipleAnswerQuestion.Question.QuestionType == QuestionType.Multiple)
            {
                await _questionsRepository.AddSingleMultipleAnswerQuestionAsync(questionAC);
                return Ok(questionAC);
            }
            else
            {
                //To-Do Modify parameter accordingly _questionsRepository.AddCodeSnippetQuestion(questionAC);
                return Ok(questionAC);
            }          
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
