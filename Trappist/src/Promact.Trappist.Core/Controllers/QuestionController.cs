using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.Repository.Questions;
using System.Threading.Tasks;
namespace Promact.Trappist.Core.Controllers
{
    [Route("api/question")]
    [Authorize]
    public class QuestionController : Controller
    {
        private readonly IQuestionRepository _questionsRepository;

        public QuestionController(IQuestionRepository questionsRepository)
        {
            _questionsRepository = questionsRepository;
        }

        #region Question API
        /// <summary>
        /// Post API to save the question
        /// </summary>
        /// <param name="questionAC">QuestionAC object</param>
        /// <returns>
        /// Returns added Question
        /// </returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddQuestion([FromBody]QuestionAC questionAC)
        {
            if (questionAC == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            if (questionAC.Question.QuestionType == QuestionType.Programming)
            {
                await _questionsRepository.AddCodeSnippetQuestionAsync(questionAC, User.Identity.Name);
            }
            else
            {
                await _questionsRepository.AddSingleMultipleAnswerQuestionAsync(questionAC, User.Identity.Name);
            }
            return Ok(questionAC);
        }

        /// <summary>
        /// API to get all the Questions
        /// </summary>
        /// <returns>Questions List</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllQuestions()
        {
            return Ok(await _questionsRepository.GetAllQuestionsAsync());
        }

        /// <summary>
        /// Returns all the coding languages
        /// </summary>
        /// <returns>
        /// Coding language object of type CodingLanguageAC
        /// </returns>
        [HttpGet("codinglanguage")]
        public async Task<IActionResult> GetAllCodingLanguages()
        {
            var codinglanguages = await _questionsRepository.GetAllCodingLanguagesAsync();
            return Ok(codinglanguages);
        }
        #endregion
    }
}