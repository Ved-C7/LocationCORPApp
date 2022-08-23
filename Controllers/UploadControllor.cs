using LocationCORPApp.Data;
using LocationCORPApp.Models;
using LocationCORPApp.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Data;
using ExcelDataReader;

namespace LocationCORPAPP.Controllers
{
    public class UploadController : Controller
    {
        private IWebHostEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _db;
        public UploadController(IWebHostEnvironment environment, ApplicationDbContext db)
        {
            _hostingEnvironment = environment;
            _db = db;
        }


        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            string uploads = Path.Combine(_hostingEnvironment.ContentRootPath, "UploadedFiles");
            if (file?.Length > 0)
            {
                string fileEnding = file.FileName.Substring(file.FileName.Length - 4);
                if (fileEnding.Contains("xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(uploads, _FileName);
                    using (Stream fileStream = new FileStream(_path, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                    List<HoltecLocations> NAMEandADDRESS = new List<HoltecLocations> { };
                    List<HoltecLocations> compareNameandAddress = new List<HoltecLocations> { };
                    compareNameandAddress.AddRange(_db.Locations.ToList());
                    List<HoltecLocations> afterSaveNameandAddress = new List<HoltecLocations> { };

                    List<LocationBuildings> locationsWithId = new List<LocationBuildings> { };
                    List<LocationBuildings> compareBuildings = new List<LocationBuildings> { };
                    compareBuildings.AddRange(_db.Buildings.ToList());
                    List<LocationBuildings> afterSaveBuildings = new List<LocationBuildings> { };

                    List<HoltecBuildingFloors> floorsWithId = new List<HoltecBuildingFloors> { };
                    List<HoltecBuildingFloors> compareFloors = new List<HoltecBuildingFloors> { };
                    compareFloors.AddRange(_db.Floors.ToList());
                    List<HoltecBuildingFloors> afterSaveFloors = new List<HoltecBuildingFloors> { };

                    List<HoltecOfficeSpaces> officesWithId = new List<HoltecOfficeSpaces> { };
                    List<HoltecOfficeSpaces> compareOffices = new List<HoltecOfficeSpaces> { };
                    compareOffices.AddRange(_db.Offices.ToList());

                    using (var stream = System.IO.File.OpenRead(_path))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream)) {
                                do
                                {
                                while (reader.Read())
                                    {
                                        var name = reader.GetValue(0)?.ToString().Trim();
                                        // Check Location DB
                                        //If record exist (ignore case) - Get Location ID
                                        //if not then insert and Get Location ID

                                        var address = reader.GetValue(1)?.ToString().Trim();
                                        //
                                        // Check Building DB
                                        //If record exist (ignore case) - Get Building ID
                                        //if not then insert and Get Building ID + location ID
                                        //if address 2 is there then add.. else keep null
                                        var address2 = reader.GetValue(2)?.ToString().Trim();

                                        var city = reader.GetValue(3)?.ToString().Trim();
                                        var state = reader.GetValue(4)?.ToString().Trim();
                                        var zip = reader.GetValue(5)?.ToString().Trim();
                                        var newHoltecLocations = new HoltecLocations()
                                        {
                                            Name = name,
                                            AddressOne = address,
                                            AddressTwo = address2,
                                            City = city,
                                            StateOrProvince = state,
                                            ZipCode = zip,

                                        };

                                    if (!compareNameandAddress.Any(x => x.Name == name && x.AddressOne == address))
                                        {
                                        if (!NAMEandADDRESS.Any(x => x.Name == name && x.AddressOne == address))
                                        { NAMEandADDRESS.Add(newHoltecLocations); }
                                        }

                                    }
                                } while (reader.NextResult());
                            _db.Locations.AddRange(NAMEandADDRESS);
                            _db.SaveChanges();
                            afterSaveNameandAddress.AddRange(_db.Locations.ToList());



                        }
                    }
                    using (var stream = System.IO.File.OpenRead(_path))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                                do
                                {
                                    while (reader.Read())
                                    {
                                        var name = reader.GetValue(6)?.ToString().Trim();
                                        var location = afterSaveNameandAddress.FirstOrDefault(x => x.Name == reader.GetValue(0).ToString().Trim());
                                        var newLocationBuildings = new LocationBuildings()
                                        {
                                            Name = name?.ToString(),
                                            LocationId = location.Id
                                        };
                                    if (!compareBuildings.Any(x => x.Name == name && x.LocationId == location.Id))
                                    {
                                        if (!locationsWithId.Any(x => x.Name == name && x.LocationId == location.Id))
                                        { locationsWithId.Add(newLocationBuildings);}
                                    }
                                    }

                                } while (reader.NextResult());
                            }
                            _db.Buildings.AddRange(locationsWithId);
                            _db.SaveChanges();
                            afterSaveBuildings.AddRange(_db.Buildings.ToList());
                    }
                    using (var stream = System.IO.File.OpenRead(_path))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            do
                            {
                                while (reader.Read())
                                {
                                        var floorNumber = Convert.ToInt64(reader.GetValue(7));
                                        var location = afterSaveNameandAddress.FirstOrDefault(x => x.Name == reader.GetValue(0).ToString().Trim());
                                        var buildingLocation = afterSaveBuildings.Where(x => x.LocationId == location.Id);
                                        var pickedBuilding = afterSaveBuildings.FirstOrDefault(x => x.LocationId == location.Id); //The definition for this variable doesn't mean anything I just have it so I can redefine it as building later
                                        foreach (var building in buildingLocation)
                                        {
                                            if (floorNumber == Convert.ToInt64(reader.GetValue(7)) && location.Name.ToString().Trim() == reader.GetValue(0).ToString().Trim() && building.Name.ToString().Trim() == reader.GetValue(6).ToString().Trim())
                                            {
                                                pickedBuilding = building;
                                            }
                                        }
                                        var newHoltecFloors = new HoltecBuildingFloors()
                                        {
                                            FloorNumber = (int)floorNumber,
                                            BuildingId = pickedBuilding.Id
                                        };
                                        if (!compareFloors.Any(x => x.FloorNumber == (int)floorNumber && x.BuildingId == pickedBuilding.Id))
                                        {
                                            if (!floorsWithId.Any(x => x.FloorNumber == (int)floorNumber && x.BuildingId == pickedBuilding.Id))
                                            { floorsWithId.Add(newHoltecFloors); }
                                        }
                                }

                            } while (reader.NextResult());
                        }
                        _db.Floors.AddRange(floorsWithId);
                        _db.SaveChanges();
                        afterSaveFloors.AddRange(_db.Floors.ToList());
                    }
                    using (var stream = System.IO.File.OpenRead(_path))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            do
                            {
                                while (reader.Read())
                                {
                                        var officeNumber = reader.GetValue(8)?.ToString().Trim();
                                        var officeType = reader.GetValue(9)?.ToString().Trim();
                                        var areaSqft = Convert.ToInt64(reader.GetValue(10));
                                        var floorNumber = Convert.ToInt64(reader.GetValue(7));
                                        var location = afterSaveNameandAddress.FirstOrDefault(x => x.Name == reader.GetValue(0).ToString().Trim());
                                        var buildingLocation = afterSaveBuildings.Where(x => x.LocationId == location.Id);
                                        var pickedBuilding = afterSaveBuildings.FirstOrDefault(x => x.LocationId == location.Id); //The definition for this variable doesn't mean anything I just have it so I can redefine it as building later
                                        var pickedFloor = afterSaveFloors.FirstOrDefault(x => x.BuildingId == pickedBuilding.Id); //The definition for this variable doesn't mean anything I just have it so I can redefine it as building later 
                                        foreach (var building in buildingLocation)
                                        {
                                            if (floorNumber == Convert.ToInt64(reader.GetValue(7)) && location.Name.ToString().Trim() == reader.GetValue(0).ToString().Trim() && building.Name.ToString().Trim() == reader.GetValue(6).ToString().Trim() && officeNumber.ToString().Trim() == reader.GetValue(8)?.ToString().Trim())
                                            {
                                                pickedBuilding = building;
                                            }
                                        }
                                        var floorLocation = afterSaveFloors.Where((x => x.BuildingId == pickedBuilding.Id));
                                        foreach (var floor in floorLocation)
                                        {
                                            if (floorNumber == floor.FloorNumber && location.Name.ToString().Trim() == reader.GetValue(0).ToString().Trim() && pickedBuilding.Name.ToString().Trim() == reader.GetValue(6).ToString().Trim() && officeNumber.ToString().Trim() == reader.GetValue(8)?.ToString().Trim())
                                            {
                                                pickedFloor = floor;
                                            }
                                        }
                                        var newHoltecOffices = new HoltecOfficeSpaces()
                                        {
                                            OfficeCode = officeNumber,
                                            FloorId = pickedFloor.Id,
                                            Name = officeType,
                                            AreaSqft = (int)areaSqft
                                        };

                                        if (!compareOffices.Any(x => x.OfficeCode == officeNumber.ToString() && x.FloorId == pickedFloor.Id))
                                        {
                                            if (!officesWithId.Any(x => x.OfficeCode == officeNumber.ToString() && x.FloorId == pickedFloor.Id))
                                            { officesWithId.Add(newHoltecOffices); }
                                        }
                                }

                            } while (reader.NextResult());
                        }
                        _db.Offices.AddRange(officesWithId);
                        _db.SaveChanges();
                    }
                    ViewBag.Message = "File Uploaded Successfully!";
                }

                else
                {
                    ViewBag.Message = "File Upload Failed!";
                }
            }
            else
            {
                ViewBag.Message = "File Upload Failed!";
            }
            return View();
        }

    }
}