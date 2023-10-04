using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace G.Models.Gdata
{
  [Table("Departments")]
  public partial class Department
  {
    [Key]
    public int DepartmentID
    {
      get;
      set;
    }

    public ICollection<Course> Courses { get; set; }
    [ConcurrencyCheck]
    public string Name
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public string StartDate
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public int? InstructorID
    {
      get;
      set;
    }
    public Instructor Instructor { get; set; }
    [ConcurrencyCheck]
    public string ConcurrencyToken
    {
      get;
      set;
    }
  }
}
