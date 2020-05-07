using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VisualAlgorithms.ViewModels;

namespace VisualAlgorithms.Models
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Algorithm>()
                .HasOne(al => al.AlgorithmTimeComplexity)
                .WithOne(atc => atc.Algorithm)
                .HasForeignKey<AlgorithmTimeComplexity>(atc => atc.AlgorithmId);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Group)
                .WithMany(g => g.Users)
                .HasForeignKey(u => u.GroupId);

            builder.Entity<Test>()
                .HasOne(t => t.Algorithm)
                .WithMany(al => al.Tests)
                .HasForeignKey(t => t.AlgorithmId);

            builder.Entity<TestQuestion>()
                .HasOne(tq => tq.Test)
                .WithMany(t => t.TestQuestions)
                .HasForeignKey(tq => tq.TestId);

            builder.Entity<TestAnswer>()
                .HasOne(ta => ta.TestQuestion)
                .WithMany(tq => tq.TestAnswers)
                .HasForeignKey(ta => ta.TestQuestionId);

            builder.Entity<UserAnswer>()
                .HasOne(ua => ua.TestQuestion)
                .WithMany(tq => tq.UserAnswers)
                .HasForeignKey(ua => ua.TestQuestionId);

            builder.Entity<UserAnswer>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.TestAnswers)
                .HasForeignKey(ua => ua.UserId);

            builder.Entity<UserTest>()
                .HasKey(ut => new { ut.TestId, ut.UserId });

            builder.Entity<UserTest>()
                .HasOne(ut => ut.Test)
                .WithMany(t => t.UserTests)
                .HasForeignKey(ut => ut.TestId);

            builder.Entity<UserTest>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UserTests)
                .HasForeignKey(ut => ut.UserId);

        }

        public DbSet<Algorithm> Algorithms { get; set; }
        public DbSet<AlgorithmTimeComplexity> AlgorithmTimeComplexities { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestQuestion> TestQuestions { get; set; }
        public DbSet<TestAnswer> TestAnswers { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<UserTest> UserTests { get; set; }
    }
}
