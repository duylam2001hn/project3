using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gallery_art_3.Models;
using PagedList;


namespace Gallery_art_3.Controllers
{
    public class artworksController : Controller
    {
        private Datacontext db = new Datacontext();

        // GET: artworks
        public ActionResult Index(int? page)
        {
            if (Session["idArtist"] != null)
            {

                if (page == null) page = 1;

                int artist_id = int.Parse(Session["idArtist"].ToString());

                var artworks = db.artworks.Where(s => s.artist_id == artist_id).OrderBy(s=>s.Id);
                int pageSize = 4;

                int pageNumber = (page ?? 1);
                return View(artworks.ToPagedList(pageNumber,pageSize));
            }
            return RedirectToAction("Login","customers");
            
        }

        // GET: artworks/Details/5
        public ActionResult Details(int? id)
        {
            int? id_artwork = id;
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            }
            artwork artwork = db.artworks.Find(id_artwork);
           ViewBag.Status = artwork_status(artwork.status);
            if (artwork == null)
            {
                return HttpNotFound();
            }
            return View(artwork);
        }

        // GET: artworks/Create
        public ActionResult Create()
        {
            if (Session["idArtist"] != null)
            {
                ViewBag.cate_id = new SelectList(db.categories, "Id", "Name");
            return View();
            }
            return RedirectToAction("Login", "customers");
        }

        // POST: artworks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Price,Description,Year,img_path,artist_id,cate_id,status")] artwork artwork, HttpPostedFileBase uploadhinh)
        {
           
                if (ModelState.IsValid)
                {
                    int id = int.Parse(Session["idArtist"].ToString());
                    artwork.artist_id = id;
                    artwork.status = 0;
                    if (uploadhinh != null && uploadhinh.ContentLength > 0)
                    {
                        var findArtist = db.artists.Where(s => s.Id.Equals(id)).ToList();
                        string nameArtist = findArtist.FirstOrDefault().customer.FullName;
                    string dt = DateTime.Now.ToString("MM_dd_yyyy_hh_mm_ss");


                    //Validate 

                    string fileName = uploadhinh.FileName; // getting File Name

                    string fileContentType = uploadhinh.ContentType; // getting ContentType

                    byte[] tempFileBytes = new byte[uploadhinh.ContentLength]; // getting filebytes

                    var data = uploadhinh.InputStream.Read(tempFileBytes, 0, Convert.ToInt32(uploadhinh.ContentLength));

                    var types = FileUploadCheck.FileType.Image; // Setting Image type

                    var result = FileUploadCheck.isValidFile(tempFileBytes, types, fileContentType); //Calling isValidFile method


                    if(result == true)
                    {
                        string _FileName = "";
                        //var type = MvcSecurity.Filters.FileUploadCheck.FileType.Image;
                        int index = uploadhinh.FileName.IndexOf('.');
                        _FileName = "Artist" + nameArtist + dt + "." + uploadhinh.FileName.Substring(index + 1);
                        string _path = Path.Combine(Server.MapPath("~/Upload/Artwork"), _FileName);
                        uploadhinh.SaveAs(_path);
                        string imgpath = "Upload/Artwork/" + _FileName;
                        artwork.img_path = imgpath;
                        db.artworks.Add(artwork);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    return RedirectToAction("Create");
                    }
                }

                ViewBag.artist_id = new SelectList(db.artists, "Id", "Certificate", artwork.artist_id);
                ViewBag.cate_id = new SelectList(db.categories, "Id", "Name", artwork.cate_id);
                return View(artwork);
           
        }

        // GET: artworks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            artwork artwork = db.artworks.Find(id);
            if (artwork == null)
            {
                return HttpNotFound();
            }
            Session["imgpath"] = artwork.img_path;
            //ViewBag.artist_id = new SelectList(db.artists, "Id", "Certificate", artwork.artist_id);
            ViewBag.cate_id = new SelectList(db.categories, "Id", "Name", artwork.cate_id);
            return View(artwork);
        }

        // POST: artworks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Price,Description,Year,artist_id,cate_id,status")] artwork artwork,HttpPostedFileBase uploadhinh)
        {
           
            if (ModelState.IsValid)
            {

                artwork edit_data = db.artworks.Find(artwork.Id);
                edit_data.cate_id = artwork.cate_id;
                edit_data.Year = artwork.Year;
                edit_data.Title = artwork.Title;
                edit_data.Description = artwork.Description;
                if(artwork.status == 0)
                {
                    int id = int.Parse(Session["idArtist"].ToString());
                    if (uploadhinh != null && uploadhinh.ContentLength > 0)
                    {
                        var findArtist = db.artists.Where(s => s.Id.Equals(id)).ToList();
                        string nameArtist = findArtist.FirstOrDefault().customer.FullName;
                        string dt = DateTime.Now.ToString("MM_dd_yyyy_hh_mm_ss");

                        string _FileName = "";
                        int index = uploadhinh.FileName.IndexOf('.');
                        _FileName = "Artist" + nameArtist + dt + "." + uploadhinh.FileName.Substring(index + 1);
                        string _path = Path.Combine(Server.MapPath("~/Upload/Artwork"), _FileName);
                        uploadhinh.SaveAs(_path);
                        string imgpath = "Upload/Artwork/" + _FileName;

                        edit_data.img_path = imgpath;
                    }
                    
                }

                db.Entry(edit_data).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            
            ViewBag.cate_id = new SelectList(db.categories, "Id", "Name", artwork.cate_id);
            return View(artwork);
        }

        // GET: artworks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            artwork artwork = db.artworks.Find(id);
            ViewBag.Status= artwork_status(artwork.status);
            if (artwork == null)
            {
                return HttpNotFound();
            }
            return View(artwork);
        }

        // POST: artworks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            artwork artwork = db.artworks.Find(id);
            db.artworks.Remove(artwork);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        

        public string artwork_status(int status)
        {

            if (status == 2)
            {
                return "Đang đấu giá";
            }
            else if (status == 1)
            {
                return "Đã bán";
            }
            else
            {
                return "Đang bán";
            }
        }
    }
}
