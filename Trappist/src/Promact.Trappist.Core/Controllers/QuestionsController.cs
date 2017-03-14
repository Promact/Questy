using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.Repository.Questions;
using System;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api/question")]
    public class QuestionsController : Controller
    {
        private readonly IQuestionsRespository _questionsRepository;
        public QuestionsController(IQuestionsRespository questionsRepository)
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
        /// Add single multiple answer question into SingleMultipleAnswerQuestion model
        /// </summary>
        /// <param name="singleMultipleAnswerQuestion"></param>
        /// <returns></returns>
        public IActionResult AddSingleMultipleAnswerQuestion(SingleMultipleAnswerQuestion singleMultipleAnswerQuestion)
        {
            _questionsRepository.AddSingleMultipleAnswerQuestion(singleMultipleAnswerQuestion);
            return Ok();
        }

        /// <summary>
        /// Add options of single multiple answer question to SingleMultipleAnswerQuestionOption model
        /// </summary>
        /// <param name="singleMultipleAnswerQuestionOption"></param>
        /// <returns></returns>
        public IActionResult AddSingleMultipleAnswerQuestionOption(SingleMultipleAnswerQuestionOption singleMultipleAnswerQuestionOption)
        {
            _questionsRepository.AddSingleMultipleAnswerQuestionOption(singleMultipleAnswerQuestionOption);
            return Ok();
        }
    }
}
