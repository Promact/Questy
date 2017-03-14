using System;
using System.ComponentModel.DataAnnotations;

namespace Promact.Trappist.DomainModel.Models.Test
{
    public class Test : BaseModel
    {
        [Required] 
        [MaxLength(150, ErrorMessage = "Test Name length Should be less than 150")] 
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
    }
}