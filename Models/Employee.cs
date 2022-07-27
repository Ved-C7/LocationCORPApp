namespace LocationCORPApp.Models
{
    public class Employee

    {

        public int EmpID { get; set; }

        public int BUID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmployeeID { get; set; }

        public string CompEmailID { get; set; }

        public string PersonalEmailID { get; set; }

        public string PrimaryPhone { get; set; }

        public DateTime DOB { get; set; }

        public bool IsActive { get; set; }

        public string Remarks { get; set; }

        public bool Cloak { get; set; }

        public string FullName
        {

            get

            {

                return this.FirstName + " " + this.LastName;

            }

        }

        public int? LocationId { get; set; }

    }
}
