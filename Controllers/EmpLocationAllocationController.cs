using LocationCORPApp.Data;
using LocationCORPApp.Models;
using LocationCORPApp.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LocationCORPApp.Controllers
{
    public class EmpLocationAllocationController : Controller
    {
        private readonly ApplicationDbContext _db;

        public EmpLocationAllocationController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<SelectListItem> LocationNames = _db.Locations
           .Select(u => new SelectListItem() { Text = u.Name, Value = u.Id.ToString() })
           .ToList();
            List<HoltecLocations> locations = new List<HoltecLocations> { };
            locations.AddRange(_db.Locations.ToList());
            List<EmployeeLocations> employeeLocations = new List<EmployeeLocations> { };
            employeeLocations.AddRange(_db.EmployeeLocation.ToList());
            var temp = employeeLocations.First(x => x.LocationId != 0);
            ViewData["CurrentLoc"] = locations.FirstOrDefault(x => x.Id == temp.LocationId).Id;

            List<LocationBuildings> buildings = new List<LocationBuildings> { };
            buildings.AddRange(_db.Buildings.ToList());
            var temp2 = employeeLocations.First(x => x.BuildingId != 0);
            ViewData["CurrentBu"] = buildings.FirstOrDefault(x => x.Id == temp.BuildingId).Id;

            ViewData["LocationNames"] = LocationNames;

            //var employeeFromDB = _db.Employee.FirstOrDefault(x => x.EmpID == 1);
            //var location = _db.EmployeeLocation.FirstOrDefault(x => x.Id == employeeFromDB.LocationId);
            //var employeeVM = new EmployeeProfileVM() {
            //    EmpID = employeeFromDB.EmpID,
              //  BUID = employeeFromDB.BUID,
                //FirstName = employeeFromDB.FirstName,
                //LastName = employeeFromDB.LastName,
                //CompEmailID = employeeFromDB.CompEmailID,
                //PersonalEmailID = employeeFromDB.PersonalEmailID,
                //DOB = employeeFromDB.DOB,
           //     IsActive = employeeFromDB.IsActive,
             //   Remarks = employeeFromDB.Remarks,
          //      Cloak = employeeFromDB.Cloak,
          //      LocationId = employeeFromDB.LocationId,
           //     EmployeeLocation = new EmployeeLocationVM {
            //         EmployeeID = (int)location.EmployeeId,
                     //LocationName = location.LocationId,
                     //BuildingName = location.BuildingId,
                     //FloorLevel = location.FloorId,
                     //OfficeCubicleNumber = location.OfficeId,
            //    }
          //  };

            return View();
        }

        [HttpPost]
        public JsonResult GetLocationName(string id)
        {
            List<SelectListItem> BuildingNames = _db.Buildings
            .Where(a => a.LocationId == Convert.ToInt32(id))
           .Select(u => new SelectListItem() { Text = u.Name, Value = u.Id.ToString() })
           .ToList();

            return Json(new SelectList(BuildingNames, "Value", "Text"));

        }
    
        public JsonResult GetFloorLevels(string id)
        {
            List<SelectListItem> FloorLevels = _db.Floors
            .Where(a => a.BuildingId == Convert.ToInt32(id))
           .Select(u => new SelectListItem() { Text = u.FloorNumber.ToString(), Value = u.Id.ToString() })
           .ToList();

            return Json(new SelectList(FloorLevels, "Value", "Text"));

        }
        public JsonResult GetOfficeNumbers(string id)
        {
            List<SelectListItem> OfficeNumbers = _db.Offices
            .Where(a => a.FloorId == Convert.ToInt32(id))
           .Select(u => new SelectListItem() { Text = u.OfficeCode + " - " + u.Name, Value = u.Id.ToString() })
           .ToList();

            return Json(new SelectList(OfficeNumbers, "Value", "Text"));

        }
        public IActionResult Submit()
        {
            List<SelectListItem> LocationNames = _db.Locations
           .Select(u => new SelectListItem() { Text = u.Name, Value = u.Id.ToString() })
           .ToList();

            ViewData["LocationNames"] = LocationNames;

            ViewData["PickedLocationName"] = Request.Form["LocationName"].ToString();
            ViewData["PickedBuildingName"] = Request.Form["BuildingName"].ToString();
            ViewData["PickedFloorLevel"] = Request.Form["FloorLevel"].ToString();
            ViewData["PickedOfficeCubicleNumber"] = Request.Form["OfficeCubicleNumber"].ToString();
            Console.WriteLine(ViewData["PickedLocationName"]);
            Console.WriteLine(ViewData["PickedBuildingName"]);
            Console.WriteLine(ViewData["PickedFloorLevel"]);
            Console.WriteLine(ViewData["PickedOfficeCubicleNumber"]);

            return View("Index");
        }

    }
}
