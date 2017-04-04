using Microsoft.Extensions.DependencyInjection;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.Repository.Questions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Promact.Trappist.Test.Questions
{
    [Collection("Register Dependency")]
    public class QuestionsRepositoryTest : BaseTest
    {
        private readonly Bootstrap _bootstrap;
        private readonly TrappistDbContext _trappistDbContext;
        private readonly IQuestionRespository _questionRepository;

        public QuestionsRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            _bootstrap = bootstrap;
            //resolve dependency to be used in tests
            _trappistDbContext = _bootstrap.ServiceProvider.GetService<TrappistDbContext>();
            _questionRepository = _bootstrap.ServiceProvider.GetService<IQuestionRespository>();
            ClearDatabase.ClearDatabaseAndSeed(_trappistDbContext);
        }

        /// <summary>
        /// Test to add new code snippet question
        /// </summary>
        [Fact]
        public async Task AddCodeSnippetQuestionAsync()
        {
            var codingQuestion = await CreateCodingQuestion();
            await _questionRepository.AddCodeSnippetQuestionAsync(codingQuestion);

            Assert.True(_trappistDbContext.Question.Count(x => x.QuestionDetail == codingQuestion.Question.QuestionDetail) == 1);
        }

        /// <summary>
        /// Creates Coding Question
        /// </summary>
        /// <returns>Created CodingQuestion object</returns>
        private async Task<QuestionAC> CreateCodingQuestion()
        {
            var categoryToCreate = CreateCategory();
            var category = await _trappistDbContext.Category.AddAsync(categoryToCreate);

            QuestionAC codingQuestion = new QuestionAC
            {
                Question = new DomainModel.Models.Question.Question
                {
                    QuestionDetail = "<h1>Write a program to add two number</h1>",
                    CategoryID = category.Entity.Id,
                    DifficultyLevel = DomainModel.Enum.DifficultyLevel.Easy,
                    QuestionType = DomainModel.Enum.QuestionType.Programming
                },
                CodeSnippetQuestionAC = new CodeSnippetQuestionAC
                {
                    CheckCodeComplexity = true,
                    CheckTimeComplexity = true,
                    RunBasicTestCase = true,
                    RunCornerTestCase = false,
                    RunNecessaryTestCase = false
                },
                SingleMultipleAnswerQuestionAC = null
            };

            return codingQuestion;
        }

        /// <summary>
        /// Creates dummy category
        /// </summary>
        /// <returns>Created Category object</returns>
        private DomainModel.Models.Category.Category CreateCategory()
        {
            var category = new DomainModel.Models.Category.Category
            {
                CategoryName = "Test Category",
                CreatedDateTime = DateTime.UtcNow
            };

            return category;
        }
    }
}
