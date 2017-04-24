using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.Repository.Questions;
using Promact.Trappist.Utility.Constants;
using Promact.Trappist.Web.Models;
using System;
using System.Threading.Tasks;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api/question")]
    [Authorize]
    public class QuestionController : Controller
    {
        #region Private Member
        private readonly IQuestionRepository _questionsRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringConstants _stringConstants;
        #endregion

        #region Constructor
        public QuestionController(IQuestionRepository questionsRepository, UserManager<ApplicationUser> userManager, IStringConstants stringConstants)
        {
            _questionsRepository = questionsRepository;
            _userManager = userManager;
            _stringConstants = stringConstants;
        }
        #endregion

        #region Question API
        /// <summary>
        /// Post API to save the question
        /// </summary>
        /// <param name="questionAC">QuestionAC object</param>
        /// <returns>
        /// Returns added Question
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> AddQuestionAsync([FromBody]QuestionAC questionAC)
        {
            if (questionAC == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var applicationUser = await _userManager.FindByEmailAsync(User.Identity.Name);

            try
            {
                if (questionAC.Question.QuestionType == QuestionType.Programming)
                {
                    await _questionsRepository.AddCodeSnippetQuestionAsync(questionAC, applicationUser.Id);
                }
                else
                {
                    await _questionsRepository.AddSingleMultipleAnswerQuestionAsync(questionAC, applicationUser.Id);
                }
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }

            return Ok(questionAC);
        }

        /// <summary>
        /// API to get all the Questions
        /// </summary>
        /// <returns>Questions List</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllQuestionsAsync()
        {
            var applicationUser = await _userManager.FindByEmailAsync(User.Identity.Name);
            return Ok(await _questionsRepository.GetAllQuestionsAsync(applicationUser.Id));
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

        /// <summary>
        /// Updates an existing Question in database.
        /// </summary>
        /// <param name="id">Id of Question to update</param>
        /// <param name="questionAC">Object of QuestionAC</param>
        /// <returns>Returns updated question</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestionAsync(int id, [FromBody] QuestionAC questionAC)
        {
            if (questionAC == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            if (!await _questionsRepository.IsQuestionExistAsync(id))
            {
                return NotFound();
            }
            var applicationUser = await _userManager.FindByEmailAsync(User.Identity.Name);

            try
            {
                if (questionAC.Question.QuestionType == QuestionType.Programming)
                {
                    await _questionsRepository.UpdateCodeSnippetQuestionAsync(id, questionAC, applicationUser.Id);
                }
                else
                {
                    await _questionsRepository.UpdateSingleMultipleAnswerQuestionAsync(id, questionAC, applicationUser.Id);
                }
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
            return Ok(questionAC);
        }

        /// <summary>
        /// API to delete Question
        /// </summary>
        /// <param name="id">Id to delete Question</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestionAsync([FromRoute]int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!await _questionsRepository.IsQuestionExistAsync(id))
            {
                return NotFound();
            }

            if (await _questionsRepository.IsQuestionExistInTestAsync(id))
            {
                ModelState.AddModelError(_stringConstants.ErrorKey, _stringConstants.QuestionExistInTestError);
                return BadRequest(ModelState);
            }

            await _questionsRepository.DeleteQuestionAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Returns Question of specific Id
        /// </summary>
        /// <param name="id">Id of Question</param>
        /// <returns>
        /// QuestionAC class object
        /// </returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestionByIdAsync([FromRoute]int id)
        {
            var questionAC = await _questionsRepository.GetQuestionByIdAsync(id);
            if (questionAC == null)
            {
                return NotFound();
            }
            return Ok(questionAC);
        }
        #endregion
    }
}