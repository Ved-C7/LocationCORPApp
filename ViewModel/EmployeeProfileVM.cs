using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using LocationCORPApp.Models;

namespace LocationCORPApp.ViewModel;
public class EmployeeProfileVM
{
    [Key]
    public int Id { get; set; }
    [Required]
    [Display(Name = "Employee Name")]
    public string Name { get; set; }
    public string Designation { get; set; }
    [DataType(DataType.MultilineText)]
    public string Address { get; set; }
    public DateTime? RecordCreatedOn { get; set; }
    public int? LocationName { get; set; }
    public EmployeeLocations EmployeeLocation { get; set; }
    public EmployeeLocationVM EmployeeLocationVM { get; set; }
}
