using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace G.Models.Gdata
{
  [Table("Enrollments")]
  public partial class Enrollment
  {
    [Key]
    public int EnrollmentID
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public int CourseID
    {
      get;
      set;
    }
    public Course Course { get; set; }
    [ConcurrencyCheck]
    public int StudentID
    {
      get;
      set;
    }
    public Student Student { get; set; }
    [ConcurrencyCheck]
    public int? Grade
    {
      get;
      set;
    }
  }
}
