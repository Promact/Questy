using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.Repository.Questions;
using Promact.Trappist.Web.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Promact.Trappist.Test.Questions
{
    [Collection("Register Dependency")]
    public class QuestionsRepositoryTest : BaseTest
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public QuestionsRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            //resolve dependency to be used in tests
            _questionRepository = _scope.ServiceProvider.GetService<IQuestionRepository>();
            _userManager = _scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            ClearDatabase.ClearDatabaseAndSeed(_trappistDbContext);
        }

        /// <summary>
        /// Test to add new code snippet question
        /// </summary>
        [Fact]
        public async Task AddCodeSnippetQuestionAsync()
        {
            string userName = "deepankar@promactinfo.com";

            ApplicationUser user = new ApplicationUser() { Email = userName, UserName = userName };
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);

            var codingQuestion = await CreateCodingQuestion();
            await _questionRepository.AddCodeSnippetQuestionAsync(codingQuestion, applicationUser.Id);

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
                Question = new QuestionDetailAC
                {
                    QuestionDetail = "<h1>Write a program to add two number</h1>",
                    CategoryID = category.Entity.Id,
                    DifficultyLevel = DomainModel.Enum.DifficultyLevel.Easy,
                    QuestionType = DomainModel.Enum.QuestionType.Programming
                },
                CodeSnippetQuestion = new CodeSnippetQuestionAC
                {
                    CheckCodeComplexity = true,
                    CheckTimeComplexity = true,
                    RunBasicTestCase = true,
                    RunCornerTestCase = false,
                    RunNecessaryTestCase = false
                },
                SingleMultipleAnswerQuestion = null
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
                CategoryName = "Test Category"
            };

            return category;
        }
    }
}
