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

        async Task<ApplicationUser?> EnsureUserAsync(string email, string fullName, string phone, string password, string role)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
                return user;

            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FullName = fullName,
                PhoneNumber = phone
            };
            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));

            await userManager.AddToRoleAsync(user, role);
            return user;
        }

        await EnsureUserAsync("admin@unimanage.local", "System Admin", "0100000000", "Admin123!", AppRoles.Administrator);
        var lecturer = await EnsureUserAsync("lecturer@unimanage.local", "Dr. Jane Lecturer", "0100000001", "Lecturer123!", AppRoles.Lecturer);
        var student = await EnsureUserAsync("student@unimanage.local", "Alex Student", "0100000002", "Student123!", AppRoles.Student);

        if (lecturer == null || student == null)
            return;

        if (!await context.Courses.AnyAsync())
        {
            var cs101 = new Course
            {
                Code = "CS101",
                Name = "Introduction to Computer Science",
                Description = "Fundamentals of programming and algorithms.",
                Credits = 4,
                EnrollmentLimit = 50,
                LecturerId = lecturer.Id,
                PrerequisiteId = null
            };
            context.Courses.Add(cs101);
            await context.SaveChangesAsync();

            var cs201 = new Course
            {
                Code = "CS201",
                Name = "Data Structures",
                Description = "Lists, trees, graphs, and complexity.",
                Credits = 4,
                EnrollmentLimit = 35,
                LecturerId = lecturer.Id,
                PrerequisiteId = cs101.CourseId
            };
            context.Courses.Add(cs201);
            await context.SaveChangesAsync();

            var assign = new Assignment
            {
                CourseId = cs101.CourseId,
                Title = "Hello World Lab",
                Description = "Submit a short program that prints your name.",
                DueDateUtc = DateTime.UtcNow.AddDays(21),
                MaxPoints = 100,
                CreatedAtUtc = DateTime.UtcNow
            };
            context.Assignments.Add(assign);

            if (!await context.Enrollments.AnyAsync(e => e.StudentId == student.Id && e.CourseId == cs101.CourseId))
            {
                context.Enrollments.Add(new Enrollment
                {
                    StudentId = student.Id,
                    CourseId = cs101.CourseId,
                    EnrolledAtUtc = DateTime.UtcNow
                });
            }

            await context.SaveChangesAsync();
        }
    }
}
