using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.Repository.Questions;
using Promact.Trappist.Web.Models;
using System.Threading.Tasks;
namespace Promact.Trappist.Core.Controllers
{
    [Route("api/question")]
    [Authorize]
    public class QuestionController : Controller
    {
        private readonly IQuestionRepository _questionsRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public QuestionController(IQuestionRepository questionsRepository, UserManager<ApplicationUser> userManager)
        {
            _questionsRepository = questionsRepository;
            _userManager = userManager;
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
        public async Task<IActionResult> AddQuestion([FromBody]QuestionAC questionAC)
        {
            if (questionAC == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var applicationUser = await _userManager.FindByEmailAsync(User.Identity.Name);

            if (questionAC.Question.QuestionType == QuestionType.Programming)
            {
                await _questionsRepository.AddCodeSnippetQuestionAsync(questionAC, applicationUser.Id);
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