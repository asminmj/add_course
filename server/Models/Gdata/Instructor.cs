using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace G.Models.Gdata
{
  [Table("Instructor")]
  public partial class Instructor
  {
    [Key]
    public int ID
    {
      get;
      set;
    }

    public ICollection<CourseInstructor> CourseInstructors { get; set; }
    public ICollection<Department> Departments { get; set; }
    public ICollection<OfficeAssignment> OfficeAssignments { get; set; }
    [ConcurrencyCheck]
    public string LastName
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public string FirstName
    {
      get;
      set;
    }
    [ConcurrencyCheck]
    public string HireDate
    {
      get;
      set;
    }
  }
}
