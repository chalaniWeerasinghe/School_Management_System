using AuthSystem.Areas.Identity.Data;
using AuthSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

public class AuthDbContext : IdentityDbContext<ApplicationUser>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Mark> Marks { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<StudentSubject> StudentSubjects { get; set; }
    public DbSet<UserSubject> UserSubjects { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Define the composite key for StudentSubject
        builder.Entity<StudentSubject>()
            .HasKey(ss => new { ss.StudentId, ss.SubjectId });

        builder.Entity<Mark>()
        .HasOne(m => m.Student)
        .WithMany()
        .HasForeignKey(m => m.StudentId);

        // Configure UserSubject Relationship
        builder.Entity<UserSubject>()
            .HasKey(us => new { us.UserId, us.SubjectId });

        builder.Entity<UserSubject>()
            .HasOne(us => us.User)
            .WithMany()
            .HasForeignKey(us => us.UserId);

        builder.Entity<UserSubject>()
            .HasOne(us => us.Subject)
            .WithMany()
            .HasForeignKey(us => us.SubjectId);

        //// Configure the relationships for StudentSubject
        //builder.Entity<StudentSubject>()
        //    .HasOne(ss => ss.Student)    // A StudentSubject has one Student
        //    .WithMany(s => s.StudentSubjects)  // A Student has many StudentSubjects
        //    .HasForeignKey(ss => ss.StudentId);  // The foreign key for StudentId

        //builder.Entity<StudentSubject>()
        //    .HasOne(ss => ss.Subject)  // A StudentSubject has one Subject
        //    .WithMany(s => s.StudentSubjects)  // A Subject has many StudentSubjects
        //    .HasForeignKey(ss => ss.SubjectId);  // The foreign key for SubjectId

        // Seed Grades data
        //builder.Entity<Grade>().HasData(
        //    new Grade { GradeId = 1, GradeName = "A" },
        //    new Grade { GradeId = 2, GradeName = "B" },
        //    new Grade { GradeId = 3, GradeName = "C" },
        //    new Grade { GradeId = 4, GradeName = "F" }
        //);
    }
}

