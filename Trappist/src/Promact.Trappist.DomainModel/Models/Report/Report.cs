﻿using Promact.Trappist.DomainModel.Enum;
using Promact.Trappist.DomainModel.Models.TestConduct;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promact.Trappist.DomainModel.Models.Report
{
    public class Report : BaseModel
    {
        public double TotalMarksScored { get; set; }

        public int TotalCorrectAttempts { get; set; }

        public double Percentage { get; set; }

        public double Percentile { get; set; }

        public TestStatus TestStatus { get; set; }

        public int TimeTakenByAttendee { get; set; }

        public int TestAttendeeId { get; set; }

        public bool IsTestPausedUnWillingly { get; set; }

        public bool IsAllowResume { get; set; }

        [ForeignKey("TestAttendeeId")]
        public virtual TestAttendees TestAttendee { get; set; }
    }
}