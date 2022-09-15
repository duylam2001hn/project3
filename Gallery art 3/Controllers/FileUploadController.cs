using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gallery_art_3.Controllers
{
    public class FileUploadController : Controller
    {
        // GET: FileUpload
        public ActionResult UploadFiles()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFiles(HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    //Method 2 Get file details from HttpPostedFileBase class    

                    if (file != null)
                    {
                        string path = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        ViewBag.FileStatus = "File uploaded successfully.";
                    }
                    ViewBag.FileStatus = "File uploaded false";

                }
                catch (Exception)
                {
                    ViewBag.FileStatus = "Error while file uploading."; ;
                }
            }
            return View("UploadFiles"); ;
        }
    }
}
