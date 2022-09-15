using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gallery_art_3.Models;

namespace Gallery_art_3.Controllers
{
    public class artistsController : Controller
    {
        private Datacontext db = new Datacontext();

        // GET: artists
        public ActionResult Index()
        {
            if (/*Session["idUser"] != null && */Session["idArtist"] != null)
            {
                var artists = db.artists.Include(a => a.customer);
                return View(artists.ToList());
            }
            return RedirectToAction("Login", "customers");
        }

        // GET: artists/Details/5
        public ActionResult Details(int? id)
        {
            int id_artist = Int32.Parse(Session["idArtist"].ToString());
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            artist artist = db.artists.Find(id_artist);
            
            var detail_artist = artist.customer.ToString();
          
            ViewBag.Cus = detail_artist;
            
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // GET: artists/Create
        public ActionResult Create()
        {
           
            return View();
        }

        // POST: artists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Certificate,Description,Style,Expire_date,Cus_id")] artist artist)
        {
          
            artist.Cus_id =Int32.Parse(Session["idUser"].ToString());
            artist.Expire_date = Session["ExpireDate"].ToString();

            if (ModelState.IsValid)
            {
                db.artists.Add(artist);
                db.SaveChanges();
                return RedirectToAction("Logout","customers");
            }
            

            return RedirectToAction("Logout", "customers");
        }

        // GET: artists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            artist artist = db.artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            ViewBag.Cus_id = new SelectList(db.customers, "Id", "FullName", artist.Cus_id);
            return View(artist);
        }

        // POST: artists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Certificate,Description,Style,Expire_date,Cus_id")] artist artist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(artist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Cus_id = new SelectList(db.customers, "Id", "FullName", artist.Cus_id);
            return View(artist);
        }

        // GET: artists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            artist artist = db.artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // POST: artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            artist artist = db.artists.Find(id);
            db.artists.Remove(artist);
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


       
    }
}
