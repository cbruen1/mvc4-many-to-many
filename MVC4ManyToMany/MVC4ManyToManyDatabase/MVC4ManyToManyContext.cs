using System.Data;
using MVC4ManyToManyDomain;
using System.Data.Entity;

namespace MVC4ManyToManyDatabase
{
    public class MVC4ManyToManyContext : DbContext
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>()
                .HasMany(up => up.Courses)
                .WithMany(course => course.UserProfiles)
                .Map(mc =>
                {
                    mc.ToTable("T_UserProfile_Course");
                    mc.MapLeftKey("UserProfileID");
                    mc.MapRightKey("CourseID");
                }
            );

            base.OnModelCreating(modelBuilder);
        }

        public void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }
    }
}
