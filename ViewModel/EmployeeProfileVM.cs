using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LocationCORPApp.ViewModel;
public class EmployeeProfileVM
{
    public int EmpID { get; set; }
    [Required(ErrorMessage = "Business Unit field is required.")]
    [DisplayName("Business Unit")]
    public int BUID { get; set; }
    [Required(ErrorMessage = "First Name field is required.")]
    [DisplayName("First Name")]

    public string FirstName { get; set; }
    [Required(ErrorMessage = "Last Name field is required.")]
    [DisplayName("Last Name")]
    public string LastName { get; set; }
    [DisplayName("Gender")]
    [Required(ErrorMessage = "Please select Gender.")]
    public int Gender { get; set; }

    [DisplayName("Employee Number")]
    [Required(ErrorMessage = "Please enter Employee Number.")]
    public string EmployeeID { get; set; }
    [DisplayName("Comp Email ID")]
    [Required(ErrorMessage = "Please enter Company Email ID.")]
    public string CompEmailID { get; set; }
    [DisplayName("Personal Email ID")]
    public string PersonalEmailID { get; set; }
    [DisplayName("Primary Phone")]
    [Required(ErrorMessage = "Please enter Primary phone.")]
    public string PrimaryPhone { get; set; }
    [DisplayName("Secondary Phone")]
    public string SecondaryPhone { get; set; }
    [Required(ErrorMessage = "Please enter Birth date.")]
    [DisplayName("Date of Birth")]
    public DateTime DOB { get; set; }
    [DisplayName("Emerg. Contact Name")]
    [Required(ErrorMessage = "Please enter Emergency contact name.")]
    public bool IsActive { get; set; }
    [DisplayName("Weekly Report is Required")]
    public string Remarks { get; set; }
    public bool Cloak { get; set; }
    [DisplayName("Created By")]

    public string FullName
    {
        get
        {
            return this.LastName + " " + this.FirstName;
        }
    }
       public int? LocationId { get; set; }
    public EmployeeLocationVM EmployeeLocation { get; set; }

}
