using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using FoolProof.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocationCORPApp.ViewModel
{
    public class EmployeeLocationVM
    {

        public int EmployeeID { get; set; }

        [NotMapped]
        public List<SelectListItem>? LocationNames { get; set; }

        //[NotEqualTo(null , ErrorMessage = "Please pick a location")]
        [Required]
        public int? LocationName { get; set; }

        public int? BuildingName { get; set; }

        public int? FloorLevel { get; set; }

        public int? OfficeCubicleNumber { get; set; }

    }
}
