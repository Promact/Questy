﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.Repository.Categories;
using Promact.Trappist.Repository.Questions;
using Promact.Trappist.Utility.Constants;
using System;
using System.Linq;
using System.Threading.Tasks;
using Promact.Trappist.DomainModel.Models;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api/question")]
    [Authorize]
    public class QuestionController : Controller
    {
        #region Private Member
        private readonly IQuestionRepository _questionsRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringConstants _stringConstants;
        #endregion

        #region Constructor
        public QuestionController(IQuestionRepository questionsRepository, UserManager<ApplicationUser> userManager, IStringConstants stringConstants, ICategoryRepository categoryRepository)
        {
            _questionsRepository = questionsRepository;
            _userManager = userManager;
            _stringConstants = stringConstants;
            _categoryRepository = categoryRepository;
        }
        #endregion

        #region Public Methods
        #region Add-Edit-Delete Question API
        /// <summary>
        /// Post API to save the question
        /// </summary>
        /// <param name="questionAC">QuestionAC object</param>
        /// <returns>
        /// Returns added Question
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> AddQuestionAsync([FromBody] QuestionAC questionAC)
        {
            if (questionAC == null || !ModelState.IsValid || questionAC.Question.CategoryID == 0)
            {
                return BadRequest();
            }

            var applicationUser = await _userManager.FindByEmailAsync(User.Identity.Name);

            try
            {
                if (questionAC.Question.QuestionType == QuestionType.Programming)
                {
                    //Check for default testcase before saving question 
                    if (questionAC.CodeSnippetQuestion.CodeSnippetQuestionTestCases.Any(x => x.TestCaseType == TestCaseType.Default))
                        await _questionsRepository.AddCodeSnippetQuestionAsync(questionAC, applicationUser.Id);
                    else
                        return BadRequest();
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
            if (await _questionsRepository.IsQuestionExistInTestAsync(id))
            {
                return BadRequest(_stringConstants.QuestionEditError);
            }
            var applicationUser = await _userManager.FindByEmailAsync(User.Identity.Name);

            try
            {
                if (questionAC.Question.QuestionType == QuestionType.Programming)
                {
                    await _questionsRepository.UpdateCodeSnippetQuestionAsync(questionAC, applicationUser.Id);
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
        public async Task<IActionResult> DeleteQuestionAsync([FromRoute] int id)
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
        #endregion

        #region GetQuestions API
        /// <summary>
        /// API to get all the Questions
        /// </summary>
        /// <returns>Questions List</returns>
        [HttpGet("{id}/{categoryId}/{difficultyLevel}/{searchQuestion?}")]
        public async Task<IActionResult> GetAllQuestionsAsync([FromRoute] int id, [FromRoute] int categoryId, [FromRoute] string difficultyLevel, [FromRoute] string searchQuestion)
        {
            var applicationUser = await _userManager.FindByEmailAsync(User.Identity.Name);

            if (categoryId != 0 && !await _categoryRepository.IsCategoryExistAsync(categoryId))
            {
                return NotFound();
            }


            if (!difficultyLevel.Equals("All") && !Enum.TryParse(difficultyLevel, true, out DifficultyLevel _))
            {
                return NotFound();
            }

            return Ok(await _questionsRepository.GetAllQuestionsAsync(applicationUser.Id, id, categoryId, difficultyLevel, searchQuestion));
        }

        /// <summary>
        /// Returns all the coding languages
        /// </summary>
        /// <returns>
        /// Coding language object of type CodingLanguageAC
        /// </returns>
        [HttpGet("codinglanguage")]
        public async Task<IActionResult> GetAllCodingLanguagesAsync()
        {
            var codinglanguages = await _questionsRepository.GetAllCodingLanguagesAsync();
            return Ok(codinglanguages);
        }

        /// <summary>
        /// Returns Question of specific Id
        /// </summary>
        /// <param name="id">Id of Question</param>
        /// <returns>
        /// QuestionAC class object
        /// </returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestionByIdAsync([FromRoute] int id)
        {
            var questionAC = await _questionsRepository.GetQuestionByIdAsync(id);
            if (questionAC == null)
            {
                return NotFound();
            }
            return Ok(questionAC);
        }

        /// <summary>
        /// Calculates the the total number of questions, number of questions difficulty wise,category wise and question which is searched by its name
        /// </summary>
        /// <param name="categoryId">Id of the selected category</param>
        /// <param name="searchQuestion">Substring of the question name</param>
        /// <returns>Total number of questions, difficulty wise questions, category wise questions</returns>
        [HttpGet("numberOfQuestions/{categoryId}/{searchQuestion?}")]
        public async Task<IActionResult> GetNumberOfQuestions([FromRoute] int categoryId, [FromRoute] string searchQuestion)
        {
            var applicationUser = await _userManager.FindByEmailAsync(User.Identity.Name);
            var question = await _questionsRepository.GetNumberOfQuestionsAsync(applicationUser.Id, categoryId, searchQuestion);
            return Ok(question);
        }
        #endregion
        #endregion
    }
}