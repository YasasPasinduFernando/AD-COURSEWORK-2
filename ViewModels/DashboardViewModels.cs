namespace AD_COURSEWORK_2.ViewModels;

public enum DashboardCalendarEventKind
{
    Deadline = 0,
    Material = 1,
    Submission = 2,
    Grade = 3
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

public class StudentDashboardViewModel
{
    public List<EnrolledCourseRow> EnrolledCourses { get; set; } = new();
    public List<DeadlineRow> UpcomingDeadlines { get; set; } = new();
    public List<GradeRow> RecentGrades { get; set; } = new();
    public int UnreadMessages { get; set; }
    public DashboardCalendarModel AssignmentCalendar { get; set; } = new();

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
    }
}

public class LecturerDashboardViewModel
{
    public List<CourseSummary> Courses { get; set; } = new();
    public int PendingSubmissions { get; set; }
    public int GradedSubmissions { get; set; }
    public int UnreadMessages { get; set; }
    public DashboardCalendarModel AssignmentCalendar { get; set; } = new();

    public class CourseSummary
    {
        public int CourseId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int EnrollmentCount { get; set; }
        public int AssignmentCount { get; set; }
    }
}

public class AdminDashboardViewModel
{
    public int UserCount { get; set; }
    public int CourseCount { get; set; }
    public int EnrollmentCount { get; set; }
    public int AssignmentCount { get; set; }
    public List<PopularCourseRow> PopularCourses { get; set; } = new();

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
