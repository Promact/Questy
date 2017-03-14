using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.Repository.Questions;
using System;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api")]
    public class QuestionsController : Controller
    {
        private readonly IQuestionRepository _questionsRepository;

        public QuestionsController(IQuestionRepository questionsRepository)
        {
            _questionsRepository = questionsRepository;
        }

        [Route("single-multiple-question")]
        [HttpPost]
        /// <summary>
        /// Add single multiple answer question into model
        /// </summary>
        /// <param name="singleMultipleAnswerQuestion"></param>
        public IActionResult AddSingleMultipleAnswerQuestion(SingleMultipleAnswerQuestion singleMultipleAnswerQuestion, SingleMultipleAnswerQuestionOption singleMultipleAnswerQuestionOption)
        {
            _questionsRepository.AddSingleMultipleAnswerQuestion(singleMultipleAnswerQuestion, singleMultipleAnswerQuestionOption);
            return Ok();
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
