using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace G.Models.Gdata
{
  [Table("Course")]
  public partial class Course
  {
    [Key]
    public int CourseID
    {
      get;
      set;
    }

    public ICollection<CourseInstructor> CourseInstructors { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; }
    [ConcurrencyCheck]
    public string Title
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public int Credits
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public int DepartmentID
    {
      get;
      set;
    }
    public Department Department { get; set; }
  }
}
