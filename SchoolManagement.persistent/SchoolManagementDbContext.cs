using Microsoft.EntityFrameworkCore;
using SchoolManagement.domain;

namespace SchoolManagement.persistent
{
    public class SchoolManagementDbContext: DbContext
    {
        public SchoolManagementDbContext(DbContextOptions<SchoolManagementDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        }

    }
