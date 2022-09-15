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
    public class bidsController : Controller
    {
        private Datacontext db = new Datacontext();

        // GET: bids
        public ActionResult Index()
        {
            var bids = db.bids.Include(b => b.artwork);
            return View(bids.ToList());
        }

        // GET: bids/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bid bid = db.bids.Find(id);
            if (bid == null)
            {
                return HttpNotFound();
            }
            return View(bid);
        }

        // GET: bids/Create
        public ActionResult Create()
        {
            
            ViewBag.Art_id = new SelectList(db.artworks, "Id", "Title");
            return View();
        }

        // POST: bids/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Start_Price,End_Price,Art_id,Date_start,Date_end,Status")] bid bid)
        {

            double day_add = double.Parse(Request["day_add"].ToString());
            string date_start = DateTime.Now.ToString();
            DateTime now = new DateTime();
            string date_end = now.AddDays(day_add).ToString();

            bid.Date_start = date_start;
            bid.Date_end = date_end;
            bid.End_Price = 0;
            if (ModelState.IsValid)
            {
                db.bids.Add(bid);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Art_id = new SelectList(db.artworks, "Id", "Title", bid.Art_id);
            return View(bid);
        }

        // GET: bids/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bid bid = db.bids.Find(id);
            if (bid == null)
            {
                return HttpNotFound();
            }
            ViewBag.Art_id = new SelectList(db.artworks, "Id", "Title", bid.Art_id);
            return View(bid);
        }

        // POST: bids/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Start_Price,End_Price,Art_id,Date_start,Date_end,Status")] bid bid)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bid).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Art_id = new SelectList(db.artworks, "Id", "Title", bid.Art_id);
            return View(bid);
        }

        // GET: bids/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bid bid = db.bids.Find(id);
            if (bid == null)
            {
                return HttpNotFound();
            }
            return View(bid);
        }

        // POST: bids/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bid bid = db.bids.Find(id);
            db.bids.Remove(bid);
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
