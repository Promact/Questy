using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.Repository.Questions;
using Promact.Trappist.Utility.Constants;
using Promact.Trappist.Web.Models;
using System;
using System.IO;
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
        [HttpGet("{id}/{categoryId}/{difficultyLevel}/{searchQuestion?}")]
        public async Task<IActionResult> GetAllQuestionsAsync([FromRoute] int id, [FromRoute] int categoryId, [FromRoute] string difficultyLevel, [FromRoute] string searchQuestion)
        {
            var applicationUser = await _userManager.FindByEmailAsync(User.Identity.Name);
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

        [HttpGet("numberOfQuestions/{categoryId}/{searchQuestion?}")]
        public async Task<IActionResult> GetNumberOfQuestions([FromRoute] int categoryId, [FromRoute] string searchQuestion)
        {
            var applicationUser = await _userManager.FindByEmailAsync(User.Identity.Name);
            var question = await _questionsRepository.GetNumberOfQuestionsAsync(applicationUser.Id, categoryId, searchQuestion);
            return Ok(question);
        }

        /// <summary>
        /// Saves the image which is dragged into CKeditor
        /// </summary>
        /// <param name="upload">File which is dragged</param>
        /// <returns>Response for CKeditor</returns>
        [HttpPost("uploadDragImage")]
        public async Task<ActionResult> SaveDraggedImageAsync(IFormFile upload)
        {
            long size = upload.Length;
            var type = upload.ContentType;
            var image_url = "";
            if (size > 0 && type.ToLower() == "image/png")
            {
                image_url = await SaveFileAsync(upload);
            }

            var response = new
            {
                Uploaded = 1,
                FileName = upload.FileName,
                Url = image_url
            };

            return Ok(response);
        }

        /// <summary>
        /// Saves the image uploaded through CKeditor
        /// </summary>
        /// <param name="upload">File uploaded</param>
        /// <param name="CKEditorFuncNum">CKeditor function number</param>
        /// <param name="CKEditor">CKeditor name</param>
        /// <param name="langCode">Language code</param>
        /// <returns></returns>
        [HttpPost("uploadImage")]
        public async Task<ActionResult> SaveImageAsync(IFormFile upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            long size = upload.Length;
            var type = upload.ContentType;
            var image_url = "";
            var vMessage = "";

            if (size <= 0 && type.ToLower() != "image/png")
            {
                vMessage = "Uploaded file is not an image. Please check the file.";
            }
            else
            {
                try
                {
                    image_url = await SaveFileAsync(upload);
                    vMessage = "The file uploaded successfully.";
                }
                catch (Exception)
                {
                    vMessage = "There was an issue uploading.";
                }
            }

            var vOutput = @"<html><body><script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + image_url + "\", \"" + vMessage + "\");</script></body></html>";
            return Content(vOutput, "text/html");
        }

        /// <summary>
        /// Returns a simple image gallery
        /// </summary>
        /// <param name="CKEditorFuncNum">CKeditor function number</param>
        /// <param name="CKEditor">CKeditor name</param>
        /// <param name="langCode">Language code</param>
        /// <returns>Simple html based image gallery</returns>
        [HttpGet("browseImage")]
        public IActionResult BrowseImage(string CKEditorFuncNum, string CKEditor, string langCode)
        {
            var vOutput = @"<html><script>function foo(url){window.parent.opener.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ",url); window.close();}</script><body>";

            var filePaths = Directory.GetFiles(@"wwwroot/uploaded_image/" + User.Identity.Name + "/");

            foreach (var img in filePaths)
            {
                var url = "'/uploaded_image/" + User.Identity.Name + "/" + Path.GetFileName(img) + "'";
                vOutput += "<a href=\"javascript: foo(" + url + "); \"><img src=" + url + " height=\"200\" width=\"200\"></img></a>";
            }

            vOutput += "</body></html>";

            return Content(vOutput, "text/html");
        }
        #endregion

        #region Private Methods
        private async Task<string> SaveFileAsync(IFormFile upload)
        {
            Directory.CreateDirectory("wwwroot/uploaded_image/" + User.Identity.Name);

            using (var stream = new FileStream("wwwroot/uploaded_image/" + User.Identity.Name + "/" + upload.FileName, FileMode.Create))
            {
                await upload.CopyToAsync(stream);
            }

            return "/uploaded_image/" + User.Identity.Name + "/" + upload.FileName;
        }
        #endregion
    }
}