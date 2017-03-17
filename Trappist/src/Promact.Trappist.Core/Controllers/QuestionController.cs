using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.ApplicationClasses;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.Repository.Questions;


namespace Promact.Trappist.Core.Controllers
{
    [Route("api")]
    public class QuestionController : Controller
    {
        private readonly IQuestionRespository _questionsRepository;
        public QuestionController(IQuestionRespository questionsRepository)
        {
            _questionsRepository = questionsRepository;
        }

        /// <summary>
        /// Add single multiple answer question into model
        /// </summary>
        /// <param name="singleMultipleQuestion"></param>
        /// <returns></returns>   
        [Route("singlemultiplequestion")]
        [HttpPost]
        public IActionResult AddSingleMultipleAnswerQuestion([FromBody]SingleMultipleQuestion singleMultipleQuestion)
        {
            _questionsRepository.AddSingleMultipleAnswerQuestion(singleMultipleQuestion.singleMultipleAnswerQuestion, singleMultipleQuestion.singleMultipleAnswerQuestionOption);
            return Ok(singleMultipleQuestion);
        }

        [HttpPost("codesnippetquestion")]
        public IActionResult AddCodeSnippetQuestion([FromBody]CodeSnippetQuestionDto codeSnippetQuestionDto)
        {
            if (codeSnippetQuestionDto == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _questionsRepository.AddCodeSnippetQuestion(codeSnippetQuestionDto);

            return Ok(codeSnippetQuestionDto);
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
    }
}
