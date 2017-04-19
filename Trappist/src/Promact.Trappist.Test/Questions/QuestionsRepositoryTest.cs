using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Promact.Trappist.DomainModel.ApplicationClasses;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.Repository.Categories;
using Promact.Trappist.Repository.Questions;
using Promact.Trappist.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Promact.Trappist.Test.Questions
{
    [Collection("Register Dependency")]
    public class QuestionsRepositoryTest : BaseTest
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public QuestionsRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            //resolve dependency to be used in tests
            _questionRepository = _scope.ServiceProvider.GetService<IQuestionRepository>();
            _categoryRepository = _scope.ServiceProvider.GetService<ICategoryRepository>();
            _userManager = _scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            ClearDatabase.ClearDatabaseAndSeed(_trappistDbContext);
        }

        /// <summary>
        ///Test to get all Questions 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllQuestionsAsyncTest()
        {
            string userName = "sandipan@promactinfo.com";

            ApplicationUser user = new ApplicationUser() { Email = userName, UserName = userName };
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);

            var codingQuestion = await CreateCodingQuestion();
            await _questionRepository.AddCodeSnippetQuestionAsync(codingQuestion, applicationUser.Id);

            var result = await _questionRepository.GetAllQuestionsAsync(applicationUser.Id);
            Assert.True(result.Count() == 1);
        }

        /// <summary>
        /// Test to add single answer Question
        /// </summary>
        [Fact]
        public async Task AddSingleAnswerQuestionAsync()
        {
            var singleAnswerQuestion = await CreateSingleAnswerQuestion();
            string userName = "vihar@promactinfo.com";
            ApplicationUser user = new ApplicationUser() { Email = userName, UserName = userName };
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);
            await _questionRepository.AddSingleMultipleAnswerQuestionAsync(singleAnswerQuestion, applicationUser.Id);
            Assert.True(_trappistDbContext.Question.Count() == 1);
            Assert.True(_trappistDbContext.SingleMultipleAnswerQuestion.Count() == 1);
            Assert.True(_trappistDbContext.SingleMultipleAnswerQuestionOption.Count() == 4);
        }

        /// <summary>
        /// Creating single answer Question
        /// </summary>
        /// <returns>Object of single answer Question</returns>
        private async Task<QuestionAC> CreateSingleAnswerQuestion()
        {
            var category = await _trappistDbContext.Category.AddAsync(CreateCategory());
            var singleAnswerQuestion = new QuestionAC()
            {
                Question = new QuestionDetailAC()
                {
                    QuestionDetail = "Question 1",
                    CategoryID = category.Entity.Id,
                    DifficultyLevel = DifficultyLevel.Hard,
                    QuestionType = QuestionType.Single
                },
                SingleMultipleAnswerQuestion = new SingleMultipleAnswerQuestionAC()
                {
                    SingleMultipleAnswerQuestion = new SingleMultipleAnswerQuestion(),
                    SingleMultipleAnswerQuestionOption = new List<SingleMultipleAnswerQuestionOption>()
                    {
                        new SingleMultipleAnswerQuestionOption()
                        {
                            IsAnswer = true,
                            Option = "A",
                        },
                        new SingleMultipleAnswerQuestionOption()
                        {
                            IsAnswer = false,
                            Option = "B",
                        },
                        new SingleMultipleAnswerQuestionOption()
                        {
                            IsAnswer = false,
                            Option = "C",
                        },
                        new SingleMultipleAnswerQuestionOption()
                        {
                            IsAnswer = false,
                            Option = "D",
                        }
                    }
                }
            };
            return singleAnswerQuestion;
        }

        /// <summary>
        /// Test to add multiple answer Question
        /// </summary>
        [Fact]
        public async Task AddMultipleAnswerQuestionAsync()
        {
            var multipleAnswerQuestion = await CreateMultipleAnswerQuestion();
            string userName = "vihar@promactinfo.com";
            ApplicationUser user = new ApplicationUser() { Email = userName, UserName = userName };
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);
            await _questionRepository.AddSingleMultipleAnswerQuestionAsync(multipleAnswerQuestion, applicationUser.Id);
            Assert.True(_trappistDbContext.Question.Count() == 1);
            Assert.True(_trappistDbContext.SingleMultipleAnswerQuestion.Count() == 1);
            Assert.True(_trappistDbContext.SingleMultipleAnswerQuestionOption.Count() == 4);
        }

        /// <summary>
        /// Creating multiple answer Question
        /// </summary>
        /// <returns>Object of multiple answer Question</returns>
        private async Task<QuestionAC> CreateMultipleAnswerQuestion()
        {
            var category = await _trappistDbContext.Category.AddAsync(CreateCategory());
            var multipleAnswerQuestion = new QuestionAC()
            {
                Question = new QuestionDetailAC()
                {
                    QuestionDetail = "Question 1",
                    CategoryID = category.Entity.Id,
                    DifficultyLevel = DifficultyLevel.Hard,
                    QuestionType = QuestionType.Multiple
                },
                SingleMultipleAnswerQuestion = new SingleMultipleAnswerQuestionAC()
                {
                    SingleMultipleAnswerQuestion = new SingleMultipleAnswerQuestion(),
                    SingleMultipleAnswerQuestionOption = new List<SingleMultipleAnswerQuestionOption>()
                    {
                        new SingleMultipleAnswerQuestionOption()
                        {
                            IsAnswer = true,
                            Option = "A",
                        },
                        new SingleMultipleAnswerQuestionOption()
                        {
                            IsAnswer = true,
                            Option = "B",
                        },
                        new SingleMultipleAnswerQuestionOption()
                        {
                            IsAnswer = false,
                            Option = "C",
                        },
                        new SingleMultipleAnswerQuestionOption()
                        {
                            IsAnswer = false,
                            Option = "D",
                        }
                    }
                }
            };
            return multipleAnswerQuestion;
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
            Assert.True(_trappistDbContext.CodeSnippetQuestionTestCases.Count() == 1);
        }

        /// <summary>
        /// Test to update code snippet question
        /// </summary>
        [Fact]
        public async Task UpdateCodeSnippetQuestionAsyncTest()
        {
            string userName = "deepankar@promactinfo.com";

            //Adding Application User
            ApplicationUser user = new ApplicationUser() { Email = userName, UserName = userName };
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);

            //Adding code snippet question
            var codingQuestion = await CreateCodingQuestion();
            codingQuestion.CodeSnippetQuestion.CodeSnippetQuestionTestCases.Add(new CodeSnippetQuestionTestCases()
            {
                TestCaseTitle = "Default Check",
                TestCaseDescription = "This case is default case",
                TestCaseMarks = 10.00,
                TestCaseType = TestCaseType.Necessary,
                TestCaseInput = "1+1",
                TestCaseOutput = "2",
            });
            await _questionRepository.AddCodeSnippetQuestionAsync(codingQuestion, applicationUser.Id);

            var question = await _trappistDbContext
                .Question
                .FirstOrDefaultAsync(x => x.QuestionDetail == codingQuestion.Question.QuestionDetail);

            var updatedQuestion = await _questionRepository.GetQuestionByIdAsync(question.Id);

            //Updating code snippet question 
            updatedQuestion.CodeSnippetQuestion.CheckCodeComplexity = false;
            updatedQuestion.CodeSnippetQuestion.CheckTimeComplexity = false;
            updatedQuestion.CodeSnippetQuestion.CodeSnippetQuestionTestCases.Remove(codingQuestion.CodeSnippetQuestion.CodeSnippetQuestionTestCases.First());
            updatedQuestion.CodeSnippetQuestion.CodeSnippetQuestionTestCases.Add(new CodeSnippetQuestionTestCases
            {
                TestCaseTitle = "New check",
                TestCaseDescription = "This is a new case",
                TestCaseMarks = 10.00,
                TestCaseType = TestCaseType.Basic,
                TestCaseInput = "1+1",
                TestCaseOutput = "2",
            });
            updatedQuestion.Question.QuestionDetail = "Updated question details";
            updatedQuestion.Question.DifficultyLevel = DifficultyLevel.Hard;
            updatedQuestion.CodeSnippetQuestion.LanguageList = new string[] { "C" };
            updatedQuestion.Question.Id = question.Id;
            await _questionRepository.UpdateCodeSnippetQuestionAsync(updatedQuestion, applicationUser.Id);

            var questionAfterUpdate = await _trappistDbContext
                .Question
                .Include(x => x.CodeSnippetQuestion)
                .ThenInclude(x => x.QuestionLanguangeMapping)
                .FirstOrDefaultAsync(x => x.Id == question.Id);

            Assert.True(string.Equals(questionAfterUpdate.QuestionDetail, updatedQuestion.Question.QuestionDetail, StringComparison.CurrentCultureIgnoreCase));
            Assert.True(questionAfterUpdate.DifficultyLevel == updatedQuestion.Question.DifficultyLevel);
            Assert.True(questionAfterUpdate.CodeSnippetQuestion.CheckCodeComplexity == updatedQuestion.CodeSnippetQuestion.CheckCodeComplexity);
            Assert.True(questionAfterUpdate.CodeSnippetQuestion.CheckTimeComplexity == updatedQuestion.CodeSnippetQuestion.CheckTimeComplexity);
            Assert.True(questionAfterUpdate.CodeSnippetQuestion.QuestionLanguangeMapping.Count == 1);
        }

        [Fact]
        public async Task GetQuestionByIdAsyncTest()
        {
            string userName = "deepankar@promactinfo.com";

            ApplicationUser user = new ApplicationUser() { Email = userName, UserName = userName };
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);

            var codingQuestion = await CreateCodingQuestion();
            await _questionRepository.AddCodeSnippetQuestionAsync(codingQuestion, applicationUser.Id);
            var multipleAnswerQuestion = await CreateMultipleAnswerQuestion();
            await _questionRepository.AddSingleMultipleAnswerQuestionAsync(multipleAnswerQuestion, applicationUser.Id);

            var codeSnippetQuestion = _trappistDbContext.Question.FirstOrDefault(x => x.QuestionDetail == codingQuestion.Question.QuestionDetail);
            var singleMultipleQuestion = _trappistDbContext.Question.FirstOrDefault(x => x.QuestionDetail == multipleAnswerQuestion.Question.QuestionDetail);

            var questionFetched = await _questionRepository.GetQuestionByIdAsync(codeSnippetQuestion.Id);
            Assert.NotNull(questionFetched);

            var nullQuestion = await _questionRepository.GetQuestionByIdAsync(-1);
            Assert.Null(nullQuestion);

            questionFetched = await _questionRepository.GetQuestionByIdAsync(singleMultipleQuestion.Id);
            Assert.NotNull(questionFetched);
        }

        /// <summary>
        /// Creates Coding Question
        /// </summary>
        /// <returns>Created CodingQuestion object</returns>
        private async Task<QuestionAC> CreateCodingQuestion()
        {
            var categoryToCreate = CreateCategory();
            await _categoryRepository.AddCategoryAsync(categoryToCreate);

            QuestionAC codingQuestion = new QuestionAC
            {
                Question = new QuestionDetailAC
                {
                    QuestionDetail = "<h1>Write a program to add two number</h1>",
                    CategoryID = categoryToCreate.Id,
                    DifficultyLevel = DifficultyLevel.Easy,
                    QuestionType = QuestionType.Programming
                },
                CodeSnippetQuestion = new CodeSnippetQuestionAC
                {
                    CheckCodeComplexity = true,
                    CheckTimeComplexity = true,
                    RunBasicTestCase = true,
                    RunCornerTestCase = false,
                    RunNecessaryTestCase = false,
                    LanguageList = new String[] { "Java", "C" },
                    CodeSnippetQuestionTestCases = new List<CodeSnippetQuestionTestCases>()
                    {
                        new CodeSnippetQuestionTestCases()
                        {
                            TestCaseTitle = "Necessary check",
                            TestCaseDescription = "This case must be successfuly passed",
                            TestCaseMarks = 10.00,
                            TestCaseType = TestCaseType.Necessary,
                            TestCaseInput = "2+2",
                            TestCaseOutput = "4",
                        }
                    }
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
