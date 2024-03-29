﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.ApplicationClasses.Test;
using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.Question;
using Promact.Trappist.Repository.Categories;
using Promact.Trappist.Repository.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Promact.Trappist.DomainModel.Models;
using Promact.Trappist.Repository.Test;
using Xunit;

namespace Promact.Trappist.Test.Questions
{
    [Collection("Register Dependency")]
    public class QuestionsRepositoryTest : BaseTest
    {
        #region Private Variables
        private readonly IQuestionRepository _questionRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITestsRepository _testRepository;
        #endregion

        #region Constructor
        public QuestionsRepositoryTest(Bootstrap bootstrap) : base(bootstrap)
        {
            //resolve dependency to be used in tests
            _questionRepository = _scope.ServiceProvider.GetService<IQuestionRepository>();
            _categoryRepository = _scope.ServiceProvider.GetService<ICategoryRepository>();
            _userManager = _scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            _testRepository = _scope.ServiceProvider.GetService<ITestsRepository>();
            //ClearDatabase.ClearDatabaseAndSeed(_trappistDbContext);
        }
        #endregion

        #region Testing Methods
        #region Public Methods
        /// <summary>
        ///Test to get all Questions 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllQuestionsAsyncTest()
        {
            string userName = "sandipan@promactinfo.com";


            ApplicationUser user = new ApplicationUser { Email = userName, UserName = userName, Name = userName};
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);
            var categoryIdArray = new List<int>();
            var arrayOfDifficulty = new[] { 0, 0, 1, 1, 2, 2, 2 };
            foreach (int i in arrayOfDifficulty)
            {
                var codingQuestion = await CreateCodingQuestion((DifficultyLevel)i);
                await _questionRepository.AddCodeSnippetQuestionAsync(codingQuestion, applicationUser.Id);
                categoryIdArray.Add(codingQuestion.Question.CategoryID);
            }

            var multipleChoiceQuestion = await CreateMultipleAnswerQuestion();
            await _questionRepository.AddSingleMultipleAnswerQuestionAsync(multipleChoiceQuestion, user.Id);

            var singleChoiceQuestion = await CreateSingleAnswerQuestion();
            await _questionRepository.AddSingleMultipleAnswerQuestionAsync(singleChoiceQuestion, user.Id);
            var result = await _questionRepository.GetAllQuestionsAsync(applicationUser.Id, 0, 0, "All", null);
            Assert.Equal(9,result.Count());
            var resultForCategory = await _questionRepository.GetAllQuestionsAsync(applicationUser.Id, 0, 7, "All", null);
            Assert.Single(resultForCategory);
            var resultWithDifficultyLevel = await _questionRepository.GetAllQuestionsAsync(applicationUser.Id, 0, 0, "Easy", null);
            Assert.Equal(2, resultWithDifficultyLevel.Count());
            var resultWithSearchInput = await _questionRepository.GetAllQuestionsAsync(applicationUser.Id, 0, 6, "All", "Write");
            Assert.Single(resultWithSearchInput);
            var singleMultipleChoiceQuestions =
                await _questionRepository.GetAllQuestionsAsync(user.Id, multipleChoiceQuestion.Question.Id, 0, "All", null);
            Assert.Equal(7,singleMultipleChoiceQuestions.Count());
        }

        /// <summary>
        /// Test to add single answer Question
        /// </summary>
        [Fact]
        public async Task AddSingleAnswerQuestionAsyncTest()
        {
            var singleAnswerQuestion = await CreateSingleAnswerQuestion();
            string userName = "vihar@promactinfo.com";
            ApplicationUser user = new ApplicationUser { Email = userName, UserName = userName, Name = userName};
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);
            await _questionRepository.AddSingleMultipleAnswerQuestionAsync(singleAnswerQuestion, applicationUser.Id);
            Assert.True(_trappistDbContext.Question.Count() == 1);
            Assert.True(_trappistDbContext.SingleMultipleAnswerQuestion.Count() == 1);
            Assert.True(_trappistDbContext.SingleMultipleAnswerQuestionOption.Count() == 4);
        }

        /// <summary>
        /// Test to update single answer question
        /// </summary>
        [Fact]
        public async Task UpdateSingleAnswerQuestionAsyncTest()
        {
            //Add single answer question
            var singleAnswerQuestion = await CreateSingleAnswerQuestion();
            string userName = "vihar@promactinfo.com";
            ApplicationUser user = new ApplicationUser { Email = userName, UserName = userName, Name = userName};
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);
            await _questionRepository.AddSingleMultipleAnswerQuestionAsync(singleAnswerQuestion, applicationUser.Id);
            var question = await _trappistDbContext.Question.FirstOrDefaultAsync(x => x.QuestionDetail == singleAnswerQuestion.Question.QuestionDetail);

            //Update single answer question
            singleAnswerQuestion.Question.Id = question!.Id;
            singleAnswerQuestion.SingleMultipleAnswerQuestion.SingleMultipleAnswerQuestionOption[0].Option = "Updated Option";
            singleAnswerQuestion.Question.QuestionDetail = "Updated Single Answer Question";
            singleAnswerQuestion.Question.DifficultyLevel = DifficultyLevel.Medium;
            singleAnswerQuestion.SingleMultipleAnswerQuestion.SingleMultipleAnswerQuestionOption.RemoveAt(1);
            singleAnswerQuestion.SingleMultipleAnswerQuestion.SingleMultipleAnswerQuestionOption.Add(new SingleMultipleAnswerQuestionOption()
            {
                CreatedDateTime = DateTime.Now,IsAnswer = false, Option = "Something"
            });
            await _questionRepository.UpdateSingleMultipleAnswerQuestionAsync(question.Id, singleAnswerQuestion, applicationUser.Id);

            var updatedSingleAnswerQuestion = await _trappistDbContext.Question.FindAsync(question.Id);
            var updatedSingleAnswerQuestionOption = await _trappistDbContext.SingleMultipleAnswerQuestionOption.Where(x => x.SingleMultipleAnswerQuestionID == question.Id).OrderBy(x=>x.Id).ToListAsync();
            Assert.True(singleAnswerQuestion.SingleMultipleAnswerQuestion.SingleMultipleAnswerQuestionOption[0].Option == updatedSingleAnswerQuestionOption[0].Option);
            Assert.True(updatedSingleAnswerQuestion!.DifficultyLevel == singleAnswerQuestion.Question.DifficultyLevel);
            Assert.True(updatedSingleAnswerQuestion.QuestionDetail == singleAnswerQuestion.Question.QuestionDetail);
        }

        /// <summary>
        /// Test to get all the Coding Language
        /// </summary>
        [Fact]
        public async Task GetAllCodingLanguagesAsyncTest()
        {
            var codingLanguages = await _questionRepository.GetAllCodingLanguagesAsync();
            Assert.True(codingLanguages.Count > 0);
        }

        /// <summary>
        /// Test to add multiple answer Question
        /// </summary>
        [Fact]
        public async Task AddMultipleAnswerQuestionAsyncTest()
        {
            var multipleAnswerQuestion = await CreateMultipleAnswerQuestion();
            string userName = "vihar@promactinfo.com";
            ApplicationUser user = new ApplicationUser { Email = userName, UserName = userName, Name = userName};
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);
            await _questionRepository.AddSingleMultipleAnswerQuestionAsync(multipleAnswerQuestion, applicationUser.Id);
            Assert.True(_trappistDbContext.Question.Count() == 1);
            Assert.True(_trappistDbContext.SingleMultipleAnswerQuestion.Count() == 1);
            Assert.True(_trappistDbContext.SingleMultipleAnswerQuestionOption.Count() == 4);
        }

        /// <summary>
        /// Test to update multiple answer question
        /// </summary>
        [Fact]
        public async Task UpdateMultipleAnswerQuestionAsyncTest()
        {
            //Add multiple answer question
            var multipleAnswerQuestion = await CreateMultipleAnswerQuestion();
            string userName = "vihar@promactinfo.com";
            ApplicationUser user = new ApplicationUser { Email = userName, UserName = userName, Name = userName};
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);
            await _questionRepository.AddSingleMultipleAnswerQuestionAsync(multipleAnswerQuestion, applicationUser.Id);
            var question = await _trappistDbContext.Question.FirstOrDefaultAsync(x => x.QuestionDetail == multipleAnswerQuestion.Question.QuestionDetail);

            //Update multiple answer question
            multipleAnswerQuestion.Question.Id = question.Id;
            multipleAnswerQuestion.SingleMultipleAnswerQuestion.SingleMultipleAnswerQuestionOption[1].Option = "Updated Option";
            multipleAnswerQuestion.Question.QuestionDetail = "Updated Multiple Answer Question";
            multipleAnswerQuestion.Question.DifficultyLevel = DifficultyLevel.Easy;
            await _questionRepository.UpdateSingleMultipleAnswerQuestionAsync(question.Id, multipleAnswerQuestion, applicationUser.Id);

            var updatedMultipleAnswerQuestion = await _trappistDbContext.Question.FindAsync(question.Id);
            var updatedMultipleAnswerQuestionOption = await _trappistDbContext.SingleMultipleAnswerQuestionOption.Where(x => x.SingleMultipleAnswerQuestionID == question.Id).OrderBy(x=>x.Id).ToListAsync();
            Assert.True(multipleAnswerQuestion.SingleMultipleAnswerQuestion.SingleMultipleAnswerQuestionOption[1].Option == updatedMultipleAnswerQuestionOption[1].Option);
            Assert.True(updatedMultipleAnswerQuestion.DifficultyLevel == multipleAnswerQuestion.Question.DifficultyLevel);
            Assert.True(updatedMultipleAnswerQuestion.QuestionDetail == multipleAnswerQuestion.Question.QuestionDetail);
        }

        /// <summary>
        /// Test to add new code snippet question
        /// </summary>
        [Fact]
        public async Task AddCodeSnippetQuestionAsyncTest()
        {
            string userName = "deepankar@promactinfo.com";
            string name = "Questy Test User";

            ApplicationUser user = new ApplicationUser { Email = userName, UserName = userName, Name = name};
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
            ApplicationUser user = new ApplicationUser { Email = userName, UserName = userName, Name = userName};
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);

            //Adding code snippet question
            var codingQuestion = await CreateCodingQuestion();
            codingQuestion.CodeSnippetQuestion.CodeSnippetQuestionTestCases.Add(new CodeSnippetQuestionTestCases
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
                .FindAsync(question.Id);
            await _trappistDbContext.Entry(questionAfterUpdate).Reference(x => x.CodeSnippetQuestion).LoadAsync();
            await _trappistDbContext.Entry(questionAfterUpdate.CodeSnippetQuestion).Collection(x => x.CodeSnippetQuestionTestCases).LoadAsync();

            Assert.True(string.Equals(questionAfterUpdate.QuestionDetail, updatedQuestion.Question.QuestionDetail, StringComparison.CurrentCultureIgnoreCase));
            Assert.True(questionAfterUpdate.DifficultyLevel == updatedQuestion.Question.DifficultyLevel);
            Assert.True(questionAfterUpdate.CodeSnippetQuestion.CheckCodeComplexity == updatedQuestion.CodeSnippetQuestion.CheckCodeComplexity);
            Assert.True(questionAfterUpdate.CodeSnippetQuestion.CheckTimeComplexity == updatedQuestion.CodeSnippetQuestion.CheckTimeComplexity);
            Assert.True(questionAfterUpdate.CodeSnippetQuestion.QuestionLanguangeMapping.Count == 1);
            Assert.True(questionAfterUpdate.CodeSnippetQuestion.CodeSnippetQuestionTestCases.Count == 2);
        }

        [Fact]
        public async Task GetQuestionByIdAsyncTest()
        {
            string userName = "deepankar@promactinfo.com";

            ApplicationUser user = new ApplicationUser { Email = userName, UserName = userName, Name = userName};
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
        /// Method to test IsQuestionExistInTestAsync() method
        /// </summary>
        [Fact]
        public async Task IsQuestionExistInTestTest()
        {
            var questionList = new List<TestQuestionAC>();
            string userName = "partha@promactinfo.com";
            ApplicationUser user = new ApplicationUser
            {
                Email = userName,
                UserName = userName,
                Name = userName
            };
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);
            //create code-snippetQuestion
            var codingQuestion = await CreateCodingQuestion();
            await _questionRepository.AddCodeSnippetQuestionAsync(codingQuestion, applicationUser.Id);
            //create single-multipleQuestion
            var multipleAnswerQuestion = await CreateMultipleAnswerQuestion();
            await _questionRepository.AddSingleMultipleAnswerQuestionAsync(multipleAnswerQuestion, applicationUser.Id);
            var testCodingQuestion = new TestQuestionAC();
            
            var codeSnippetquestion = await _trappistDbContext.Question.FirstOrDefaultAsync(x => x.QuestionDetail == codingQuestion.Question.QuestionDetail);
            testCodingQuestion.Id = codeSnippetquestion.Id;
            testCodingQuestion.CategoryID = codeSnippetquestion.CategoryID;
            testCodingQuestion.IsSelect = true;
            var singleMultipleQuestion = await _trappistDbContext.Question.FirstOrDefaultAsync(x => x.QuestionDetail == multipleAnswerQuestion.Question.QuestionDetail);
            multipleAnswerQuestion.Question.Id = singleMultipleQuestion.Id;
            questionList.Add(testCodingQuestion);
            //create Test
            var test = CreateTest("DemoTest");
            await _testRepository.CreateTestAsync(test, applicationUser.Id);
            await _testRepository.AddTestQuestionsAsync(questionList, test.Id);
            Assert.True(await _questionRepository.IsQuestionExistInTestAsync(codeSnippetquestion.Id));
            //If Question doesnot exist in test
            Assert.False(await _questionRepository.IsQuestionExistInTestAsync(multipleAnswerQuestion.Question.Id));
        }

        /// <summary>
        /// Method to test IsQuestionExistAsync() method
        /// </summary>
        [Fact]
        public async Task IsQuestionExistTest()
        {
            string userName = "partha@promactinfo.com";
            ApplicationUser user = new ApplicationUser
            {
                Email = userName,
                UserName = userName,
                Name = userName
            };
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);
            //create code-snippetQuestion
            var codingQuestion = await CreateCodingQuestion();
            await _questionRepository.AddCodeSnippetQuestionAsync(codingQuestion, applicationUser.Id);

            var questionId = (await _trappistDbContext.Question.SingleAsync(x => x.QuestionDetail == codingQuestion.Question.QuestionDetail)).Id;
            Assert.True(await _questionRepository.IsQuestionExistAsync(questionId));
        }

        /// <summary>
        /// Method to test delete Question
        /// </summary>
        [Fact]
        public async Task DeleteQuestionTest()
        {
            string userName = "user@domain.com";
            ApplicationUser user = new ApplicationUser { Email = userName, UserName = userName, Name = userName};
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);
            //Create code-snippet Question
            var codingQuestion = await CreateCodingQuestion();
            await _questionRepository.AddCodeSnippetQuestionAsync(codingQuestion, applicationUser.Id);
            //Delete code-snippet Question
            var questionId = (await _trappistDbContext.Question.SingleAsync(x => x.QuestionDetail == codingQuestion.Question.QuestionDetail)).Id;
            await _questionRepository.DeleteQuestionAsync(questionId);
            //Add single-multiple Question
            var multipleAnswerQuestion = await CreateMultipleAnswerQuestion();
            await _questionRepository.AddSingleMultipleAnswerQuestionAsync(multipleAnswerQuestion, applicationUser.Id);
            //Delete single-Multiple Question
            questionId = (await _trappistDbContext.Question.SingleAsync(x => x.QuestionDetail == multipleAnswerQuestion.Question.QuestionDetail)).Id;
            await _questionRepository.DeleteQuestionAsync(questionId);
            //True single-multiple & code-snippet Questions are both deleted 
            Assert.True(!_trappistDbContext.Question.Any());
        }

        /// <summary>
        /// Test Case for getting number of questions difficulty wise
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetNumberOfQuestionTest()
        {
            string userName = "asif@gmail.com";
            ApplicationUser user = new ApplicationUser { Email = userName, UserName = userName, Name = userName};
            await _userManager.CreateAsync(user);
            var applicationUser = await _userManager.FindByEmailAsync(user.Email);
            int categoryId = 0;
            var arrayOfDifficulty = new[] {0, 0, 1, 1, 2, 2, 2 };
            foreach (int i in arrayOfDifficulty)
            {
                var codingQuestion = await CreateCodingQuestion((DifficultyLevel)i);
                await _questionRepository.AddCodeSnippetQuestionAsync(codingQuestion, applicationUser.Id);
                categoryId = codingQuestion.Question.CategoryID;
            }
            //var codingQuestion = await CreateCodingQuestion(DifficultyLevel.Easy);
            //await _questionRepository.AddCodeSnippetQuestionAsync(codingQuestion, applicationUser.Id);

            var numberOfQuestions = await _questionRepository.GetNumberOfQuestionsAsync(applicationUser.Id, 0, null);
            Assert.Equal(2, numberOfQuestions.EasyCount);
            Assert.Equal(2, numberOfQuestions.MediumCount);
            Assert.Equal(3, numberOfQuestions.HardCount);
            var numberOfQuestionsWithCategory = await _questionRepository.GetNumberOfQuestionsAsync(applicationUser.Id, categoryId, "Write");
            Assert.Equal(0, numberOfQuestionsWithCategory.EasyCount);
            Assert.Equal(0, numberOfQuestionsWithCategory.MediumCount);
            Assert.Equal(1, numberOfQuestionsWithCategory.HardCount);

        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Creates Coding Question
        /// </summary>
        /// <returns>Created CodingQuestion object</returns>
        private async Task<QuestionAC> CreateCodingQuestion(DifficultyLevel difficultyLevel = DifficultyLevel.Easy)
        {
            var categoryToCreate = CreateCategory();
            await _categoryRepository.AddCategoryAsync(categoryToCreate);

            QuestionAC codingQuestion = new QuestionAC
            {
                Question = new QuestionDetailAC
                {
                    QuestionDetail = "<h1>Write a program to add two number</h1>",
                    CategoryID = categoryToCreate.Id,
                    DifficultyLevel = difficultyLevel,
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
                    CodeSnippetQuestionTestCases = new List<CodeSnippetQuestionTestCases>
                    {
                        new()
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

        private DomainModel.Models.Test.Test CreateTest(string testName)
        {
            var test = new DomainModel.Models.Test.Test
            {
                TestName = testName,
                BrowserTolerance = 0
            };
            return test;
        }

        /// <summary>
        /// Creating multiple answer Question
        /// </summary>
        /// <returns>Object of multiple answer Question</returns>
        private async Task<QuestionAC> CreateMultipleAnswerQuestion()
        {
            var category = await _trappistDbContext.Category.AddAsync(CreateCategory());
            var multipleAnswerQuestion = new QuestionAC
            {
                Question = new QuestionDetailAC
                {
                    QuestionDetail = "Question 1",
                    CategoryID = category.Entity.Id,
                    DifficultyLevel = DifficultyLevel.Hard,
                    QuestionType = QuestionType.Multiple,
                    Id = Random.Shared.Next(0, 100)
                },
                SingleMultipleAnswerQuestion = new SingleMultipleAnswerQuestionAC
                {
                    SingleMultipleAnswerQuestion = new SingleMultipleAnswerQuestion(),
                    SingleMultipleAnswerQuestionOption = new List<SingleMultipleAnswerQuestionOption>
                    {
                        new()
                        {
                            IsAnswer = true,
                            Option = "A",
                        },
                        new()
                        {
                            IsAnswer = true,
                            Option = "B",
                        },
                        new()
                        {
                            IsAnswer = false,
                            Option = "C",
                        },
                        new()
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
        /// Creating single answer Question
        /// </summary>
        /// <returns>Object of single answer Question</returns>
        private async Task<QuestionAC> CreateSingleAnswerQuestion()
        {
            var category = await _trappistDbContext.Category.AddAsync(CreateCategory());
            var singleAnswerQuestion = new QuestionAC
            {
                Question = new QuestionDetailAC
                {
                    QuestionDetail = "Question 1",
                    CategoryID = category.Entity.Id,
                    DifficultyLevel = DifficultyLevel.Hard,
                    QuestionType = QuestionType.Single
                },
                SingleMultipleAnswerQuestion = new SingleMultipleAnswerQuestionAC
                {
                    SingleMultipleAnswerQuestion = new SingleMultipleAnswerQuestion(),
                    SingleMultipleAnswerQuestionOption = new List<SingleMultipleAnswerQuestionOption>
                    {
                        new()
                        {
                            IsAnswer = true,
                            Option = "A",
                        },
                        new()
                        {
                            IsAnswer = false,
                            Option = "B",
                        },
                        new()
                        {
                            IsAnswer = false,
                            Option = "C",
                        },
                        new()
                        {
                            IsAnswer = false,
                            Option = "D",
                        }
                    }
                }
            };
            return singleAnswerQuestion;
        }
        #endregion
        #endregion
    }
}