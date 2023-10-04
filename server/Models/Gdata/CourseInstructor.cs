using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace G.Models.Gdata
{
  [Table("CourseInstructor")]
  public partial class CourseInstructor
  {
    [Key]
    public int CoursesCourseID
    {
      get;
      set;
    }
    public Course Course { get; set; }
    [ConcurrencyCheck]
    public int InstructorsID
    {
      get;
      set;
    }
    public Instructor Instructor { get; set; }
  }
}
