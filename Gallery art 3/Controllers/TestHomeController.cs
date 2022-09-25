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
        public ActionResult Artwork(string search)
        {
            var data = db.artworks.ToList().Where(s => s.status.Equals(0));
             if (!String.IsNullOrEmpty(search)){
                data = data.Where(
                    s=>s.Title.Contains(search) || 
                       s.Description.Contains(search) ||
                       s.artist.customer.FullName.Contains(search)
                    );
            }

            return View(data);
        }

        public ActionResult Auction(string search)
        {
            var data = db.bids.Where(s=>s.artwork.status.Equals(2) ||
            s.artwork.status.Equals(1)
            
            ).ToList();

            
            if (!String.IsNullOrEmpty(search))
            {
                data = data.Where(
                    s => s.artwork.Title.Contains(search) ||
                       s.artwork.Description.Contains(search) ||
                       s.artwork.artist.customer.FullName.Contains(search)
                    ).ToList();
            }
            return View(data);
        }

        public void filter_Price()
        {
            int Price_first = 500;
            int Price_second = 1000;
            int Price_third = 1500;
            int Price_last = 2000;

            var price = db.artworks;
        }

      

       

      
       
    }
}