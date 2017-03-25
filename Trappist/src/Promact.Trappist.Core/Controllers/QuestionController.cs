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
                await _questionsRepository.AddCodeSnippetQuestionAsync(questionAC);
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
        /// coding language of CodingLanguageAC type
        /// </returns>
        [HttpGet("codinglanguage")]
        public async Task<IActionResult> GetAllCodingLanguage()
        {
            var codinglanguages = await _questionsRepository.GetAllCodingLanguageAsync();
            return Ok(codinglanguages);
        }
    }
}