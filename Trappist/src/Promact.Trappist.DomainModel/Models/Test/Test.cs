using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.TestConduct;
using Promact.Trappist.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promact.Trappist.DomainModel.Models.Test
{
    public class Test : BaseModel
    {
        [Required]
        [MaxLength(150, ErrorMessage = "Test Name length should be less than 150")]
        [RegularExpression("^[a-zA-Z0-9_ @]*$", ErrorMessage = "Test name should be alphanumeric. Allowed special symbols are @, _ and white space")]
        public string TestName { get; set; }
        public string Link { get; set; }
        public int BrowserTolerance { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public int WarningTime { get; set; }
        public string FromIpAddress { get; set; }
        public string ToIpAddress { get; set; }
        public string WarningMessage { get; set; }
        public decimal CorrectMarks { get; set; }
        public decimal IncorrectMarks { get; set; }
        public TestOrder QuestionOrder { get; set; }
        public TestOrder OptionOrder { get; set; }
        public virtual ICollection<TestAttendees> TestAttendees { get; set; }
        public virtual ICollection<TestQuestion> TestQuestion { get; set; }
        public virtual ICollection<TestCategory> TestCategory { get; set; }
        public string CreatedByUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual ApplicationUser CreatedByUser { get; set; }
    }
}
