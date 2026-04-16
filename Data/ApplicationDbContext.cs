using AD_COURSEWORK_2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AD_COURSEWORK_2.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Assignment> Assignments => Set<Assignment>();
    public DbSet<Submission> Submissions => Set<Submission>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<CourseMaterial> CourseMaterials => Set<CourseMaterial>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Course>(e =>
        {
            e.Property(x => x.Description).HasColumnType("longtext");
            e.HasIndex(x => x.Code).IsUnique();
            e.HasOne(x => x.Lecturer)
                .WithMany(x => x.TeachingCourses)
                .HasForeignKey(x => x.LecturerId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Prerequisite)
                .WithMany(x => x.DependentCourses)
                .HasForeignKey(x => x.PrerequisiteId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Enrollment>(e =>
        {
            e.HasIndex(x => new { x.StudentId, x.CourseId }).IsUnique();
            e.HasOne(x => x.Student)
                .WithMany(x => x.Enrollments)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Course)
                .WithMany(x => x.Enrollments)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Assignment>(e =>
        {
            e.Property(x => x.Description).HasColumnType("longtext");
            e.HasOne(x => x.Course)
                .WithMany(x => x.Assignments)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Submission>(e =>
        {
            e.Property(x => x.TextContent).HasColumnType("longtext");
            e.Property(x => x.Feedback).HasColumnType("longtext");
            e.HasIndex(x => new { x.AssignmentId, x.StudentId }).IsUnique();
            e.HasOne(x => x.Assignment)
                .WithMany(x => x.Submissions)
                .HasForeignKey(x => x.AssignmentId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Student)
                .WithMany(x => x.Submissions)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.GradedBy)
                .WithMany()
                .HasForeignKey(x => x.GradedById)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Message>(e =>
        {
            e.Property(x => x.Content).HasColumnType("longtext");
            e.HasIndex(x => new { x.ReceiverId, x.IsRead });
            e.HasIndex(x => x.SentAtUtc);
            e.HasOne(x => x.Sender)
                .WithMany(x => x.SentMessages)
                .HasForeignKey(x => x.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Receiver)
                .WithMany(x => x.ReceivedMessages)
                .HasForeignKey(x => x.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<CourseMaterial>(e =>
        {
            e.HasOne(x => x.Course)
                .WithMany(x => x.Materials)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.UploadedBy)
                .WithMany(x => x.UploadedMaterials)
                .HasForeignKey(x => x.UploadedById)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
