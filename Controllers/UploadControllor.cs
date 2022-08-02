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
        public UploadController(IWebHostEnvironment environment)
        {
            _hostingEnvironment = environment;
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
                    using (var stream = System.IO.File.OpenRead(Path.Combine(_hostingEnvironment.ContentRootPath, "UploadedFiles")))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {

                            do
                            {
                                while (reader.Read())
                                {
                                    reader.GetValue(0);
                                }
                            } while (reader.NextResult());

                        }
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