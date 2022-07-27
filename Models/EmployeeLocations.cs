using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LocationCORPApp.Models
{
    public class EmployeeLocations

    {

        public int Id { get; set; }

        public int? EmployeeId { get; set; }

        public int? LocationId { get; set; }

        public int? BuildingId { get; set; }

        public int? FloorId { get; set; }

        public int? OfficeId { get; set; }
    }

}
