using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

using G.Models.Gdata;

namespace G.Data
{
  public partial class GdataContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public GdataContext(DbContextOptions<GdataContext> options):base(options)
    {
    }

    public GdataContext()
    {
    }

    partial void OnModelBuilding(ModelBuilder builder);

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<G.Models.Gdata.Course>()
              .HasOne(i => i.Department)
              .WithMany(i => i.Courses)
              .HasForeignKey(i => i.DepartmentID)
              .HasPrincipalKey(i => i.DepartmentID);
        builder.Entity<G.Models.Gdata.CourseInstructor>()
              .HasOne(i => i.Course)
              .WithMany(i => i.CourseInstructors)
              .HasForeignKey(i => i.CoursesCourseID)
              .HasPrincipalKey(i => i.CourseID);
        builder.Entity<G.Models.Gdata.CourseInstructor>()
              .HasOne(i => i.Instructor)
              .WithMany(i => i.CourseInstructors)
              .HasForeignKey(i => i.InstructorsID)
              .HasPrincipalKey(i => i.ID);
        builder.Entity<G.Models.Gdata.Department>()
              .HasOne(i => i.Instructor)
              .WithMany(i => i.Departments)
              .HasForeignKey(i => i.InstructorID)
              .HasPrincipalKey(i => i.ID);
        builder.Entity<G.Models.Gdata.Enrollment>()
              .HasOne(i => i.Course)
              .WithMany(i => i.Enrollments)
              .HasForeignKey(i => i.CourseID)
              .HasPrincipalKey(i => i.CourseID);
        builder.Entity<G.Models.Gdata.Enrollment>()
              .HasOne(i => i.Student)
              .WithMany(i => i.Enrollments)
              .HasForeignKey(i => i.StudentID)
              .HasPrincipalKey(i => i.ID);
        builder.Entity<G.Models.Gdata.OfficeAssignment>()
              .HasOne(i => i.Instructor)
              .WithMany(i => i.OfficeAssignments)
              .HasForeignKey(i => i.InstructorID)
              .HasPrincipalKey(i => i.ID);

        builder.Entity<G.Models.Gdata.Department>()
              .Property(p => p.ConcurrencyToken)
              .HasDefaultValueSql("'00000000-0000-0000-0000-000000000000'");

        this.OnModelBuilding(builder);
    }


    public DbSet<G.Models.Gdata.Course> Courses
    {
      get;
      set;
    }

    public DbSet<G.Models.Gdata.CourseInstructor> CourseInstructors
    {
      get;
      set;
    }

    public DbSet<G.Models.Gdata.Department> Departments
    {
      get;
      set;
    }

    public DbSet<G.Models.Gdata.Enrollment> Enrollments
    {
      get;
      set;
    }

    public DbSet<G.Models.Gdata.Instructor> Instructors
    {
      get;
      set;
    }

    public DbSet<G.Models.Gdata.OfficeAssignment> OfficeAssignments
    {
      get;
      set;
    }

    public DbSet<G.Models.Gdata.Student> Students
    {
      get;
      set;
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
    }
  }
}
