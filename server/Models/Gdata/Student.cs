using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace G.Models.Gdata
{
  [Table("Student")]
  public partial class Student
  {
    [Key]
    public int ID
    {
      get;
      set;
    }

    public ICollection<Enrollment> Enrollments { get; set; }
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
    public string EnrollmentDate
    {
      get;
      set;
    }
  }
}
