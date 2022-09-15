using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using Gallery_art_3.Models;
using System.Net;

namespace Gallery_art_3.Controllers
{
    public class TestHomeController : Controller
    {
        // GET: TestHome

        private Datacontext db = new Datacontext();
        public ActionResult Index()
        {
           ;

            //foreach (var artwork in )
            //{
            //    ViewBag.img = artwork.img_path;
            //    ViewBag.title = artwork.Title;
            //    ViewBag.Price = artwork.Price;
            //    ViewBag.Artist = artwork.artist.customer.FullName;
            //    ViewBag.Year = artwork.Year;
            //}    
            return View(db.artworks.ToList());
        }

        public ActionResult Auction()
        {
            var bid = db.bids.ToList();

            foreach (var bids in bid)
            {
                ViewBag.img = bids.artwork.img_path;
                ViewBag.title = bids.artwork.Title;
                ViewBag.Year = bids.artwork.Year;
                ViewBag.StartPrice = bids.Start_Price;
                ViewBag.Artist = bids.artwork.artist.customer.FullName;
            }
            return View();
        }
    }
}