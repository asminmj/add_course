using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace G.Models.Gdata
{
  [Table("OfficeAssignments")]
  public partial class OfficeAssignment
  {
    [Key]
    public int InstructorID
    {
      get;
      set;
    }
    public Instructor Instructor { get; set; }
    [ConcurrencyCheck]
    public string Location
    {
      get;
      set;
    }
  }
}
