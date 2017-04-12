using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Enum;

namespace Promact.Trappist.Web.Migrations
{
    [DbContext(typeof(TrappistDbContext))]
    partial class TrappistDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Promact.Trappist.DomainModel.Models.Category.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<DateTime>("CreatedDateTime");

                    b.Property<DateTime?>("UpdateDateTime");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("Promact.Trappist.DomainModel.Models.Question.CodeSnippetQuestion", b =>
                {
                    b.Property<int>("Id");

                    b.Property<bool>("CheckCodeComplexity");

                    b.Property<bool>("CheckTimeComplexity");

                    b.Property<DateTime>("CreatedDateTime");

                    b.Property<bool>("RunBasicTestCase");

                    b.Property<bool>("RunCornerTestCase");

                    b.Property<bool>("RunNecessaryTestCase");

                    b.Property<DateTime?>("UpdateDateTime");

                    b.HasKey("Id");

                    b.ToTable("CodeSnippetQuestion");
                });

            modelBuilder.Entity("Promact.Trappist.DomainModel.Models.Question.CodeSnippetQuestionTestCases", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CodeSnippetQuestionId");

                    b.Property<DateTime>("CreatedDateTime");

                    b.Property<string>("TestCaseDescription");

                    b.Property<string>("TestCaseInput")
                        .IsRequired();

                    b.Property<double>("TestCaseMarks");

                    b.Property<string>("TestCaseOutput")
                        .IsRequired();

                    b.Property<string>("TestCaseTitle")
                        .IsRequired();

                    b.Property<int>("TestCaseType");

                    b.Property<DateTime?>("UpdateDateTime");

                    b.HasKey("Id");

                    b.HasIndex("CodeSnippetQuestionId");

                    b.ToTable("CodeSnippetQuestionTestCases");
                });

            modelBuilder.Entity("Promact.Trappist.DomainModel.Models.Question.CodingLanguage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDateTime");

                    b.Property<string>("Language")
                        .IsRequired();

                    b.Property<DateTime?>("UpdateDateTime");

                    b.HasKey("Id");

                    b.ToTable("CodingLanguage");
                });

            modelBuilder.Entity("Promact.Trappist.DomainModel.Models.Question.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CategoryID");

                    b.Property<string>("CreatedByUserId");

                    b.Property<DateTime>("CreatedDateTime");

                    b.Property<int>("DifficultyLevel");

                    b.Property<string>("QuestionDetail")
                        .IsRequired();

                    b.Property<int>("QuestionType");

                    b.Property<DateTime?>("UpdateDateTime");

                    b.Property<string>("UpdatedByUserId");

                    b.HasKey("Id");

                    b.HasIndex("CategoryID");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("UpdatedByUserId");

                    b.ToTable("Question");
                });

            modelBuilder.Entity("Promact.Trappist.DomainModel.Models.Question.QuestionLanguageMapping", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDateTime");

                    b.Property<int>("LanguageId");

                    b.Property<int>("QuestionId");

                    b.Property<DateTime?>("UpdateDateTime");

                    b.HasKey("Id");

                    b.HasIndex("LanguageId");

                    b.HasIndex("QuestionId");

                    b.ToTable("QuestionLanguageMapping");
                });

            modelBuilder.Entity("Promact.Trappist.DomainModel.Models.Question.SingleMultipleAnswerQuestion", b =>
                {
                    b.Property<int>("Id");

                    b.Property<DateTime>("CreatedDateTime");

                    b.Property<DateTime?>("UpdateDateTime");

                    b.HasKey("Id");

                    b.ToTable("SingleMultipleAnswerQuestion");
                });

            modelBuilder.Entity("Promact.Trappist.DomainModel.Models.Question.SingleMultipleAnswerQuestionOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDateTime");

                    b.Property<bool>("IsAnswer");

                    b.Property<string>("Option")
                        .IsRequired();

                    b.Property<int>("SingleMultipleAnswerQuestionID");

                    b.Property<DateTime?>("UpdateDateTime");

                    b.HasKey("Id");

                    b.HasIndex("SingleMultipleAnswerQuestionID");

                    b.ToTable("SingleMultipleAnswerQuestionOption");
                });

            modelBuilder.Entity("Promact.Trappist.DomainModel.Models.Test.Test", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BrowserTolerance");

                    b.Property<decimal>("CorrectMarks");

                    b.Property<DateTime>("CreatedDateTime");

                    b.Property<int>("Duration");

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("FromIpAddress");

                    b.Property<decimal>("IncorrectMarks");

                    b.Property<string>("Link");

                    b.Property<string>("OptionOrder");

                    b.Property<string>("QuestionOrder");

                    b.Property<DateTime>("StartDate");

                    b.Property<string>("TestName")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("ToIpAddress");

                    b.Property<DateTime?>("UpdateDateTime");

                    b.Property<string>("WarningMessage");

                    b.Property<int>("WarningTime");

                    b.HasKey("Id");

                    b.ToTable("Test");
                });

            modelBuilder.Entity("Promact.Trappist.DomainModel.Models.TestConduct.TestAttendees", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContactNumber")
                        .HasMaxLength(15);

                    b.Property<DateTime>("CreatedDateTime");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("RollNumber")
                        .IsRequired();

                    b.Property<int>("TestId");

                    b.Property<DateTime?>("UpdateDateTime");

                    b.HasKey("Id");

                    b.HasIndex("TestId");

                    b.ToTable("TestAttendees");
                });

            modelBuilder.Entity("Promact.Trappist.Web.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("CreatedDateTime");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("OrganizationName")
                        .HasMaxLength(150);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<DateTime?>("UpdatedDateTime");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Promact.Trappist.Web.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Promact.Trappist.Web.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Promact.Trappist.Web.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Promact.Trappist.DomainModel.Models.Question.CodeSnippetQuestion", b =>
                {
                    b.HasOne("Promact.Trappist.DomainModel.Models.Question.Question", "Question")
                        .WithOne("CodeSnippetQuestion")
                        .HasForeignKey("Promact.Trappist.DomainModel.Models.Question.CodeSnippetQuestion", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Promact.Trappist.DomainModel.Models.Question.CodeSnippetQuestionTestCases", b =>
                {
                    b.HasOne("Promact.Trappist.DomainModel.Models.Question.CodeSnippetQuestion", "CodeSnippetQuestion")
                        .WithMany("CodeSnippetQuestionTestCases")
                        .HasForeignKey("CodeSnippetQuestionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Promact.Trappist.DomainModel.Models.Question.Question", b =>
                {
                    b.HasOne("Promact.Trappist.DomainModel.Models.Category.Category", "Category")
                        .WithMany("Question")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Promact.Trappist.Web.Models.ApplicationUser", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId");

                    b.HasOne("Promact.Trappist.Web.Models.ApplicationUser", "UpdatedByUser")
                        .WithMany()
                        .HasForeignKey("UpdatedByUserId");
                });

            modelBuilder.Entity("Promact.Trappist.DomainModel.Models.Question.QuestionLanguageMapping", b =>
                {
                    b.HasOne("Promact.Trappist.DomainModel.Models.Question.CodingLanguage", "CodeLanguage")
                        .WithMany("QuestionLanguangeMapping")
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Promact.Trappist.DomainModel.Models.Question.CodeSnippetQuestion", "CodeSnippetQuestion")
                        .WithMany("QuestionLanguangeMapping")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Promact.Trappist.DomainModel.Models.Question.SingleMultipleAnswerQuestion", b =>
                {
                    b.HasOne("Promact.Trappist.DomainModel.Models.Question.Question", "Question")
                        .WithOne("SingleMultipleAnswerQuestion")
                        .HasForeignKey("Promact.Trappist.DomainModel.Models.Question.SingleMultipleAnswerQuestion", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Promact.Trappist.DomainModel.Models.Question.SingleMultipleAnswerQuestionOption", b =>
                {
                    b.HasOne("Promact.Trappist.DomainModel.Models.Question.SingleMultipleAnswerQuestion", "SingleMultipleAnswerQuestion")
                        .WithMany("SingleMultipleAnswerQuestionOption")
                        .HasForeignKey("SingleMultipleAnswerQuestionID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Promact.Trappist.DomainModel.Models.TestConduct.TestAttendees", b =>
                {
                    b.HasOne("Promact.Trappist.DomainModel.Models.Test.Test", "Test")
                        .WithMany("TestAttendees")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
