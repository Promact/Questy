namespace Promact.Trappist.DomainModel.ApplicationClasses.Reports
{
    public class ReportQuestionsCountAC
    {
        public int TestAttendeeId { get; set; }
        public int EasyQuestionAttempted { get; set; }
        public int MediumQuestionAttempted { get; set; }
        public int HardQuestionAttempted { get; set; }
        public int CorrectQuestionsAttempted { get; set; }
        public int NoOfQuestionAttempted { get; set; }


    }
}
