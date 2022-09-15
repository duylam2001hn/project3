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

namespace Gallery_art_3.Controllers
{
    public class artworksController : Controller
    {
        private Datacontext db = new Datacontext();

        // GET: artworks
        public ActionResult Index()
        {
            int status = 0;
            var artwork = db.artworks.ToList();

            foreach (var item in artwork)
            {
                status = item.status;
            }

            artwork_status(status);

            var artworks = db.artworks.Include(a => a.artist).Include(a => a.category);
           
            return View(artworks.ToList());
        }

        // GET: artworks/Details/5
        public ActionResult Details(int? id)
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
                        string _FileName = "";
                        int index = uploadhinh.FileName.IndexOf('.');
                        _FileName = "Artist" + nameArtist + "." + uploadhinh.FileName.Substring(index + 1);
                        string _path = Path.Combine(Server.MapPath("~/Upload/Artwork"), _FileName);
                        uploadhinh.SaveAs(_path);
                        string imgpath = "Upload/Artwork/" + _FileName;
                        artwork.img_path = imgpath;
                        db.artworks.Add(artwork);
                        db.SaveChanges();
                        return RedirectToAction("Index");
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
            ViewBag.artist_id = new SelectList(db.artists, "Id", "Certificate", artwork.artist_id);
            ViewBag.cate_id = new SelectList(db.categories, "Id", "Name", artwork.cate_id);
            return View(artwork);
        }

        // POST: artworks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Price,Description,Year,img_path,artist_id,cate_id,status")] artwork artwork)
        {
            if (ModelState.IsValid)
            {
                db.Entry(artwork).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.artist_id = new SelectList(db.artists, "Id", "Certificate", artwork.artist_id);
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
            artwork_status(artwork.status);
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

        public void artwork_status(int status)
        {

            
            if(status == 2)
            {
                ViewBag.Status = "Đang đấu giá";
            }else if(status == 1)
            {
                ViewBag.Status = "Đã bán";
            }
            else
            {
                ViewBag.Status = "Đang bán";
            }
        } 
    }
}
