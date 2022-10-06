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
           
                var artists = db.artists.Include(a => a.customer);
                return View(artists.ToList());
           
        }

        // GET: artists/Details/5
        public ActionResult Details(int? id)
        {
        if (Session["idArtist"] != null)
            {
                int id_artist = Int32.Parse(Session["idArtist"].ToString());
               
                artist artist = db.artists.Find(id_artist);
            
                var detail_artist = artist.customer.ToString();
          
                ViewBag.Cus = detail_artist;
            
                if (artist == null)
                {
                    return HttpNotFound();
                }
                return View(artist);
            }
            return RedirectToAction("Logout", "customers");
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
        public ActionResult Create([Bind(Include = "Id,Certificate,Description,Style,Expire_date,Cus_id,City,Address,Country")] artist artist)
        {
          
            artist.Cus_id =int.Parse(Session["idUser"].ToString());
            artist.Expire_date = Session["ExpireDate"].ToString();

            if (ModelState.IsValid)
            {
                db.artists.Add(artist);
                db.SaveChanges();
                
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
            
            return View(artist);
        }

        // POST: artists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Certificate,Description,Style,Expire_date,Cus_id")] artist artist, [Bind(Exclude = "Password")] customer customer )
        {
           
            if (ModelState.IsValid)
            {
                artist edit_artist = db.artists.Find(artist.Id);
                edit_artist.Certificate = artist.Certificate;
                edit_artist.Description = artist.Description;
                edit_artist.Style = artist.Style;

                int id_cus = int.Parse(Session["idUser"].ToString());
                customer edit_customer = db.customers.Find(id_cus);
                edit_customer.Address = customer.Address;
                edit_customer.FullName = customer.FullName;
                edit_customer.Email = customer.Email;
                edit_customer.City = customer.City;

                db.Entry(edit_customer).State = EntityState.Modified;
                db.Entry(edit_artist).State = EntityState.Modified;
                db.SaveChanges();
               return RedirectToAction("Details");
            }
            
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
