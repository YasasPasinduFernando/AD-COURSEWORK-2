namespace AD_COURSEWORK_2.ViewModels;

public enum DashboardCalendarEventKind
{
    Deadline = 0,
    Material = 1,
    Submission = 2,
    Grade = 3,
    Meeting = 4
}

public class DashboardCalendarModel
{
    public int Year { get; set; }
    public int Month { get; set; }
    public Dictionary<DateOnly, List<DashboardCalendarEvent>> EventsByDay { get; set; } = new();
}

public class DashboardCalendarEvent
{
    public string Title { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public DashboardCalendarEventKind Kind { get; set; } = DashboardCalendarEventKind.Deadline;
}

public class CourseStudentSubmissionRow
{
    public int AssignmentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime DueDateUtc { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal? Grade { get; set; }
    public decimal MaxPoints { get; set; }
}

public class TrendPoint
{
    public string Label { get; set; } = string.Empty;
    public double Value { get; set; }
}

public class ActivityFeedItem
{
    public string Icon { get; set; } = "bi-clock-history";
    public string Tone { get; set; } = "info";
    public string Title { get; set; } = string.Empty;
    public string? Detail { get; set; }
    public DateTime WhenUtc { get; set; }
}

public class UpcomingMeetingRow
{
    public int MeetingId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CourseCode { get; set; } = string.Empty;
    public string LecturerName { get; set; } = string.Empty;
    public DateTime ScheduledAtUtc { get; set; }
    public int DurationMinutes { get; set; }
    public string MeetingUrl { get; set; } = string.Empty;
    public bool IsMine { get; set; }
}

public class StudentDashboardViewModel
{
    public List<EnrolledCourseRow> EnrolledCourses { get; set; } = new();
    public List<DeadlineRow> UpcomingDeadlines { get; set; } = new();
    public List<GradeRow> RecentGrades { get; set; } = new();
    public int UnreadMessages { get; set; }
    public DashboardCalendarModel AssignmentCalendar { get; set; } = new();

    public List<TrendPoint> GradeTrend { get; set; } = new();
    public int CountNotStarted { get; set; }
    public int CountSubmitted { get; set; }
    public int CountGraded { get; set; }
    public int CountOverdue { get; set; }
    public List<ActivityFeedItem> Activity { get; set; } = new();
    public List<UpcomingMeetingRow> UpcomingMeetings { get; set; } = new();

    public class EnrolledCourseRow
    {
        public int CourseId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Lecturer { get; set; } = string.Empty;
    }

    public class DeadlineRow
    {
        public int AssignmentId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
        public DateTime DueDateUtc { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class GradeRow
    {
        public string CourseCode { get; set; } = string.Empty;
        public string Assignment { get; set; } = string.Empty;
        public decimal? Grade { get; set; }
        public decimal MaxPoints { get; set; }
        public string? FeedbackShort { get; set; }
        public DateTime? GradedAtUtc { get; set; }
    }
}

public class LecturerDashboardViewModel
{
    public List<CourseSummary> Courses { get; set; } = new();
    public int PendingSubmissions { get; set; }
    public int GradedSubmissions { get; set; }
    public int UnreadMessages { get; set; }
    public DashboardCalendarModel AssignmentCalendar { get; set; } = new();

    public List<TrendPoint> SubmissionsPerCourse { get; set; } = new();
    public List<TrendPoint> GradedPerCourse { get; set; } = new();
    public List<RecentSubmissionRow> RecentSubmissions { get; set; } = new();
    public List<ActivityFeedItem> Activity { get; set; } = new();
    public List<UpcomingMeetingRow> UpcomingMeetings { get; set; } = new();

    public class CourseSummary
    {
        public int CourseId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int EnrollmentCount { get; set; }
        public int AssignmentCount { get; set; }
    }

    public class RecentSubmissionRow
    {
        public int SubmissionId { get; set; }
        public int AssignmentId { get; set; }
        public string AssignmentTitle { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? SubmittedAtUtc { get; set; }
    }
}

public class AdminDashboardViewModel
{
    public int UserCount { get; set; }
    public int CourseCount { get; set; }
    public int EnrollmentCount { get; set; }
    public int AssignmentCount { get; set; }
    public List<PopularCourseRow> PopularCourses { get; set; } = new();

    public int StudentCount { get; set; }
    public int LecturerCount { get; set; }
    public int AdminCount { get; set; }
    public int SubmissionCount { get; set; }
    public int MessageCount { get; set; }

    public List<TrendPoint> EnrollmentTrend { get; set; } = new();
    public List<ActivityFeedItem> RecentActivity { get; set; } = new();

    public class PopularCourseRow
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Enrollments { get; set; }
    }
}

public class ReportCoursePopularityRow
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int EnrollmentCount { get; set; }
    public int Capacity { get; set; }
    public double FillRate { get; set; }
}

public class ReportStudentPerformanceRow
{
    public string StudentName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int GradedCount { get; set; }
    public double? AveragePercent { get; set; }
}

public class ReportLecturerWorkloadRow
{
    public string LecturerName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int CourseCount { get; set; }
    public int AssignmentCount { get; set; }
    public int SubmissionCount { get; set; }
}

public class ReportEnrollmentRow
{
    public DateTime DateUtc { get; set; }
    public string Student { get; set; } = string.Empty;
    public string CourseCode { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
}

public class ReportPassFailRow
{
    public string CourseCode { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
    public int GradedCount { get; set; }
    public int PassCount { get; set; }
    public int FailCount { get; set; }
    public double PassRate => GradedCount > 0 ? (double)PassCount * 100 / GradedCount : 0;
    public double AveragePercent { get; set; }
}

public class ReportPassFailViewModel
{
    public List<ReportPassFailRow> Rows { get; set; } = new();
    public int PassMark { get; set; } = 50;
    public int TotalGraded => Rows.Sum(r => r.GradedCount);
    public int TotalPass => Rows.Sum(r => r.PassCount);
    public int TotalFail => Rows.Sum(r => r.FailCount);
    public double OverallPassRate => TotalGraded > 0 ? (double)TotalPass * 100 / TotalGraded : 0;
}

public class ReportAttendanceRow
{
    public int AssignmentId { get; set; }
    public string CourseCode { get; set; } = string.Empty;
    public string AssignmentTitle { get; set; } = string.Empty;
    public DateTime DueDateUtc { get; set; }
    public int Enrolled { get; set; }
    public int Submitted { get; set; }
    public int Missing => Math.Max(0, Enrolled - Submitted);
    public double AttendanceRate => Enrolled > 0 ? (double)Submitted * 100 / Enrolled : 0;
}

public class ReportAttendanceViewModel
{
    public List<ReportAttendanceRow> Rows { get; set; } = new();
    public int TotalEnrolledAcross => Rows.Sum(r => r.Enrolled);
    public int TotalSubmittedAcross => Rows.Sum(r => r.Submitted);
    public int TotalMissingAcross => Rows.Sum(r => r.Missing);
    public double OverallAttendance => TotalEnrolledAcross > 0
        ? (double)TotalSubmittedAcross * 100 / TotalEnrolledAcross
        : 0;
}
