using AD_COURSEWORK_2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AD_COURSEWORK_2.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await context.Database.MigrateAsync();

        foreach (var role in new[] { AppRoles.Student, AppRoles.Lecturer, AppRoles.Administrator })
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        async Task<ApplicationUser?> EnsureUserAsync(
            string userName,
            string email,
            string fullName,
            string phone,
            string password,
            string role,
            DateOnly? dateOfBirth = null)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = userName,
                    Email = email,
                    EmailConfirmed = true,
                    FullName = fullName,
                    PhoneNumber = phone,
                    DateOfBirth = dateOfBirth
                };
                var result = await userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                    throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
            }
            else
            {
                var updated = false;

                if (!user.EmailConfirmed)
                {
                    user.EmailConfirmed = true;
                    updated = true;
                }

                // Safe username alignment for existing seed users:
                // only update if target username is unused or already belongs to this user.
                if (!string.Equals(user.UserName, userName, StringComparison.OrdinalIgnoreCase))
                {
                    var existingByName = await userManager.FindByNameAsync(userName);
                    if (existingByName == null || existingByName.Id == user.Id)
                    {
                        user.UserName = userName;
                        user.NormalizedUserName = userManager.NormalizeName(userName);
                        updated = true;
                    }
                }

                if (updated)
                {
                    var updateResult = await userManager.UpdateAsync(user);
                    if (!updateResult.Succeeded)
                        throw new InvalidOperationException(string.Join("; ", updateResult.Errors.Select(e => e.Description)));
                }
            }

            if (!await userManager.IsInRoleAsync(user, role))
                await userManager.AddToRoleAsync(user, role);

            return user;
        }

        var admin = await EnsureUserAsync("admin", "admin@gmail.com", "System Administrator", "0770000000", "Admin123!", AppRoles.Administrator);
        var lecturer = await EnsureUserAsync("lecturer", "lecturer@gmail.com", "Dr. Nimal Perera", "0771234567", "Lecturer123!", AppRoles.Lecturer, new DateOnly(1982, 4, 12));
        var student = await EnsureUserAsync("lmsstudent", "lms@gmail.com", "Yasas Pasindu Fernando", "0712345678", "Student123!", AppRoles.Student, new DateOnly(2004, 8, 20));

        if (admin == null || lecturer == null || student == null)
            return;

        async Task<Course> EnsureCourseAsync(
            string code,
            string name,
            string description,
            int credits,
            int enrollmentLimit = 120)
        {
            var course = await context.Courses.FirstOrDefaultAsync(c => c.Code == code);
            if (course != null)
            {
                if (course.LecturerId != lecturer.Id)
                {
                    course.LecturerId = lecturer.Id;
                    await context.SaveChangesAsync();
                }
                return course;
            }

            course = new Course
            {
                Code = code,
                Name = name,
                Description = description,
                Credits = credits,
                EnrollmentLimit = enrollmentLimit,
                LecturerId = lecturer.Id,
                PrerequisiteId = null
            };
            context.Courses.Add(course);
            await context.SaveChangesAsync();
            return course;
        }

        async Task EnsureEnrollmentAsync(Course course)
        {
            var exists = await context.Enrollments
                .AnyAsync(e => e.StudentId == student.Id && e.CourseId == course.CourseId);
            if (!exists)
            {
                context.Enrollments.Add(new Enrollment
                {
                    StudentId = student.Id,
                    CourseId = course.CourseId,
                    EnrolledAtUtc = DateTime.UtcNow
                });
                await context.SaveChangesAsync();
            }
        }

        async Task<Assignment> EnsureAssignmentAsync(
            Course course,
            string title,
            string description,
            DateTime dueDateUtc,
            decimal maxPoints)
        {
            var assignment = await context.Assignments
                .FirstOrDefaultAsync(a => a.CourseId == course.CourseId && a.Title == title);
            if (assignment != null)
                return assignment;

            assignment = new Assignment
            {
                CourseId = course.CourseId,
                Title = title,
                Description = description,
                DueDateUtc = dueDateUtc,
                MaxPoints = maxPoints,
                CreatedAtUtc = DateTime.UtcNow
            };
            context.Assignments.Add(assignment);
            await context.SaveChangesAsync();
            return assignment;
        }

        async Task EnsureSubmissionAsync(
            Assignment assignment,
            SubmissionStatus status,
            decimal? grade = null,
            string? feedback = null,
            int submittedDaysOffset = -2)
        {
            var existing = await context.Submissions.FirstOrDefaultAsync(s =>
                s.AssignmentId == assignment.AssignmentId && s.StudentId == student.Id);
            if (existing != null)
                return;

            var submission = new Submission
            {
                AssignmentId = assignment.AssignmentId,
                StudentId = student.Id,
                Status = status,
                TextContent = "Seeded demo submission for coursework testing."
            };

            if (status != SubmissionStatus.NotSubmitted)
                submission.SubmittedAtUtc = assignment.DueDateUtc.AddDays(submittedDaysOffset);

            if (status == SubmissionStatus.Graded)
            {
                submission.Grade = grade;
                submission.Feedback = feedback;
                submission.GradedAtUtc = DateTime.UtcNow;
                submission.GradedById = lecturer.Id;
            }

            context.Submissions.Add(submission);
            await context.SaveChangesAsync();
        }

        static DateTime ToUtcFromSriLanka(int year, int month, int day, int hour, int minute)
        {
            // Sri Lanka is UTC+05:30, no DST.
            return new DateTime(year, month, day, hour, minute, 0, DateTimeKind.Utc).AddHours(-5.5);
        }

        async Task EnsureMeetingAsync(Course course, string title, string description, DateTime scheduledAtUtc, int durationMinutes)
        {
            var exists = await context.Meetings.AnyAsync(m =>
                m.CourseId == course.CourseId &&
                m.Title == title &&
                m.ScheduledAtUtc == scheduledAtUtc);
            if (exists)
                return;

            var meetCode = Guid.NewGuid().ToString("N")[..10];
            context.Meetings.Add(new Meeting
            {
                CourseId = course.CourseId,
                LecturerId = lecturer.Id,
                Title = title,
                Description = description,
                ScheduledAtUtc = scheduledAtUtc,
                DurationMinutes = durationMinutes,
                MeetingUrl = $"https://meet.google.com/{meetCode}"
            });
            await context.SaveChangesAsync();
        }

        var ase601 = await EnsureCourseAsync(
            "ASE601",
            "Advanced Software Engineering",
            "Covers advanced software engineering concepts, project management, software processes, requirements, architecture, testing, quality, and professional development practices.",
            credits: 20);

        var ad602 = await EnsureCourseAsync(
            "AD602",
            "Application Development",
            "Practical application development using ASP.NET Core MVC, authentication, database integration, role-based access, reporting, and deployment.",
            credits: 20);

        var ai603 = await EnsureCourseAsync(
            "AI603",
            "Artificial Intelligence",
            "Covers intelligent systems, search techniques, knowledge representation, machine learning concepts, decision-making, and AI applications.",
            credits: 20);

        var pap604 = await EnsureCourseAsync(
            "PAP604",
            "Project Analysis and Practice",
            "Covers project analysis, professional practice, academic writing, ethics, planning, reflection, and evidence-based project documentation.",
            credits: 20);

        var ip605 = await EnsureCourseAsync(
            "IP605",
            "Individual Project",
            "Final individual project module covering proposal, planning, design, implementation, testing, evaluation, final report, and viva/demo.",
            credits: 40);

        foreach (var course in new[] { ase601, ad602, ai603, pap604, ip605 })
            await EnsureEnrollmentAsync(course);

        var aseCw1 = await EnsureAssignmentAsync(
            ase601,
            "Coursework 1: Software Engineering Management Report",
            "Report covering software engineering management, planning, process selection, quality, and risk considerations.",
            new DateTime(2026, 5, 15, 23, 59, 0, DateTimeKind.Utc),
            40);
        await EnsureAssignmentAsync(
            ase601,
            "Coursework 2: Project Plan, Costing, and Quality Evaluation",
            "Project planning evidence including Gantt chart, network planning, costing, quality, and evaluation.",
            new DateTime(2026, 5, 30, 23, 59, 0, DateTimeKind.Utc),
            60);

        var adCw1 = await EnsureAssignmentAsync(
            ad602,
            "Coursework 1: ASP.NET Core LMS Implementation",
            "Implementation of the LMS using ASP.NET Core MVC, Identity authentication, database integration, role-based features, and deployment.",
            new DateTime(2026, 5, 20, 23, 59, 0, DateTimeKind.Utc),
            60);
        await EnsureAssignmentAsync(
            ad602,
            "Coursework 2: Technical Report and Demonstration Evidence",
            "Technical report, screenshots, testing evidence, deployment evidence, and demonstration preparation.",
            new DateTime(2026, 5, 30, 23, 59, 0, DateTimeKind.Utc),
            40);

        var aiCw1 = await EnsureAssignmentAsync(
            ai603,
            "Weekly Mind Map Portfolio",
            "Portfolio of weekly AI mind maps covering lecture topics, concepts, examples, images, and references.",
            new DateTime(2026, 5, 15, 23, 59, 0, DateTimeKind.Utc),
            40);
        await EnsureAssignmentAsync(
            ai603,
            "AI Game / Intelligent Agent Coursework",
            "Development and documentation of an AI-based game or intelligent agent system with behaviour logic and evaluation.",
            new DateTime(2026, 5, 28, 23, 59, 0, DateTimeKind.Utc),
            60);

        await EnsureAssignmentAsync(
            pap604,
            "Reflective Practice Report",
            "Reflective academic report covering project experience, professional practice, ethical awareness, and learning outcomes.",
            new DateTime(2026, 5, 18, 23, 59, 0, DateTimeKind.Utc),
            40);
        await EnsureAssignmentAsync(
            pap604,
            "Professional Development Portfolio",
            "Portfolio evidence covering planning, communication, professional development, and project-related learning.",
            new DateTime(2026, 5, 25, 23, 59, 0, DateTimeKind.Utc),
            60);

        await EnsureAssignmentAsync(
            ip605,
            "Interim Report and Progress Review",
            "Interim project documentation, progress review evidence, supervisor feedback, and development progress.",
            new DateTime(2026, 4, 22, 23, 59, 0, DateTimeKind.Utc),
            30);
        var ipFinal = await EnsureAssignmentAsync(
            ip605,
            "Final Report, Implementation, and Viva",
            "Final project report, completed implementation, testing evidence, evaluation, and viva/demo preparation.",
            new DateTime(2026, 5, 31, 23, 59, 0, DateTimeKind.Utc),
            70);

        await EnsureSubmissionAsync(
            adCw1,
            SubmissionStatus.Submitted,
            submittedDaysOffset: -1);
        await EnsureSubmissionAsync(
            aiCw1,
            SubmissionStatus.Submitted,
            submittedDaysOffset: -2);
        await EnsureSubmissionAsync(
            aseCw1,
            SubmissionStatus.Graded,
            grade: 75,
            feedback: "Good progress with clear evidence. Improve evaluation detail and include more testing screenshots.",
            submittedDaysOffset: -3);
        await EnsureSubmissionAsync(
            ipFinal,
            SubmissionStatus.NotSubmitted);

        await EnsureMeetingAsync(
            ase601,
            "ASE601 Lecture Session - Project Planning",
            "Software Engineering Top-Up session (Academic Year 2025/2026, Semester 2).",
            ToUtcFromSriLanka(2026, 5, 8, 18, 0),
            120);
        await EnsureMeetingAsync(
            ad602,
            "AD602 Lecture Session - LMS Implementation",
            "Application Development evening session (Academic Year 2025/2026, Semester 2).",
            ToUtcFromSriLanka(2026, 5, 9, 18, 0),
            120);
        await EnsureMeetingAsync(
            ai603,
            "AI603 Lecture Session - Intelligent Agents",
            "Artificial Intelligence weekend session (Academic Year 2025/2026, Semester 2).",
            ToUtcFromSriLanka(2026, 5, 10, 9, 0),
            180);
        await EnsureMeetingAsync(
            pap604,
            "PAP604 Lecture Session - Reflective Writing",
            "Project Analysis and Practice evening session (Academic Year 2025/2026, Semester 2).",
            ToUtcFromSriLanka(2026, 5, 11, 18, 0),
            120);
        await EnsureMeetingAsync(
            ip605,
            "IP605 Supervision Session - Viva Readiness",
            "Individual Project supervision session (Academic Year 2025/2026, Semester 2).",
            ToUtcFromSriLanka(2026, 5, 12, 19, 0),
            60);
    }
}
