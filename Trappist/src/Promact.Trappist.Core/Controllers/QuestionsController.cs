using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.Repository.Questions;
using System;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api/question")]
    public class QuestionsController : Controller
    {
        private readonly IQuestionRepository _questionsRepository;

        public QuestionsController(IQuestionRepository questionsRepository)
        {
            _questionsRepository = questionsRepository;
        }

        [HttpGet]
        /// <summary>
        /// Gets all questions
        /// </summary>
        /// <returns>Questions list</returns>   
        public IActionResult GetQuestions()
        {
            var questions = _questionsRepository.GetAllQuestions();
            return Json(questions);
        }
        [HttpPost]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="singleMultipleAnswerQuestion"></param>
        /// <returns></returns>
        public IActionResult AddSingleMultipleAnswerQuestion(SingleMultipleAnswerQuestion singleMultipleAnswerQuestion)
        {
            _questionsRepository.AddSingleMultipleAnswerQuestion(singleMultipleAnswerQuestion);
            return Ok();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="singleMultipleAnswerQuestion"></param>
        /// <returns></returns>
        public IActionResult AddSingleMultipleAnswerQuestionOption(SingleMultipleAnswerQuestionOption singleMultipleAnswerQuestionOption)
        {
            _questionsRepository.AddSingleMultipleAnswerQuestionOption(singleMultipleAnswerQuestionOption);
            return Ok();
        }
    }
}
