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

        // GET: bids/Details/5
        public ActionResult Details(int? id)
        {
            bid bid = db.bids.Find(id);
            int result = TimeBid(id);
            if (result > 0 && bid.artwork.status==2)
            {
                var danhsach = db.update_bidding
                .OrderByDescending(s => s.Amount).ToList()
                .Where(s => s.Bid_id.Equals(id));

                var email_to = danhsach.FirstOrDefault().customer.Email;
                var end_price = danhsach.FirstOrDefault().Amount;
                var cus_id = danhsach.FirstOrDefault().Cus_id;

                var callbackUrl = Url.Action("ThanhToan", "bids", new { total = end_price,cus_id = cus_id,bid_id=id }, protocol: Request.Url.Scheme);
                string content = "From Gallery Art. Complete your bills by clicking <a href=\"" + callbackUrl + "\">here</a>";
                new MailHelper().SendMail(email_to, "Thanh Toán Hóa đơn đấu giá", content);
                Update_end_bidding(id);
            }
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            if (bid == null)
            {
                return HttpNotFound();
            }
            return View(bid);
        }

        // GET: bids/Create
        public ActionResult Create()
        {
            
            int id_Artist =int.Parse(Session["idArtist"].ToString());
            var artwork = db.artworks.Where(a => a.artist_id.Equals(id_Artist));
            ViewBag.Art_id = new SelectList(artwork.Where(s=>s.status==0), "Id", "Title");
            
            return View();
        }

        // POST: bids/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Start_Price,End_Price,Art_id,Date_start,Date_end")] bid bid)
        {
            int id_artwork =bid.Art_id;
            double day_add = double.Parse(Request["add_day"].ToString());
            string date_start = DateTime.Now.ToString();
            DateTime now = DateTime.Now;
            string date_end = now.AddDays(day_add).ToString();

            bid.Date_start = date_start;
            bid.Date_end = date_end;
            bid.End_Price = 0;
            bid.Status = 0;
            
            if (ModelState.IsValid)
            {
                change_Status_Artwork(id_artwork);
                db.bids.Add(bid);
                db.SaveChanges();
                return RedirectToAction("Index","artworks");
            }

            ViewBag.Art_id = new SelectList(db.artworks, "Id", "Title", bid.Art_id);
            return View(bid);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpGet]
        public JsonResult GetArtwork_id(int id)
        {
            var artwork = db.artworks.Find(id);
            string img = artwork.img_path;
            string src = " \"/"+img+"\"";
            string html = "<img src="+src+ " style= \"width:200px;height:200px \" &frasl>"; 
            return Json(html, JsonRequestBehavior.AllowGet);
        }

        public void change_Status_Artwork (int id_artwork)
        {
            var artwork = db.artworks.Find(id_artwork);
            artwork.status = 2;
            db.Entry(artwork).State = EntityState.Modified;
            db.SaveChanges();
        }
        [HttpPost]
        public ActionResult Update_bidding()
        {
            if (Session["idUser"] != null)
            {
                Update_bidding update_Bidding = new Update_bidding();
                int idbid = int.Parse(Request["id"].ToString());
                
                    int idcus = int.Parse(Session["idUser"].ToString());

                    string giathau = Request["giathau"].ToString();
                    update_Bidding.Cus_id = idcus;
                    update_Bidding.Bid_id = idbid;
                    update_Bidding.Amount = giathau;
                    update_Bidding.Time_update = DateTime.Now.ToString();
                    db.update_bidding.Add(update_Bidding);

                int result = TimeBid(idbid);
                if(result < 0)
                {
                    db.SaveChanges();
                }
                    
                
                return RedirectToAction("Details", new { id = idbid });
            }
            return RedirectToAction("Login", "customers");
        }

        [HttpGet]
        public JsonResult update_bid_new(int id)
        {
            var danhsach = db.update_bidding
                .OrderByDescending(s => s.Amount).ToList()
                .Where(s => s.Bid_id.Equals(id));

            List<Update_bidding> data = new List<Update_bidding>();
            
            foreach (var item in danhsach)
            { 
                
                Update_bidding update = new Update_bidding();
                update.Amount = item.Amount;
                update.Cus_id = item.Cus_id;
                update.Bid_id = item.Bid_id;
                update.Time_update = item.Time_update;
                data.Add(update);
            }
            var data_new = data.Distinct(new compareList());
            
            int i = 1;
           
            var html = "";
            html += "<table border='1px'>";
            foreach (var item in data_new)
            {
                var name = db.customers.Find(item.Cus_id);
                html += "<tr>";
                html += "<td>"+i+"</td>";
                i += 1;
                html += "<td>" + name.FullName + "</td> ";
                html += "<td>" + item.Amount + "</td> ";
                html += "<td>"+ item.Time_update + "</td>";
                html += "</tr>";
            }

            html += "</table>";
            
            return Json(html, JsonRequestBehavior.AllowGet);
        }

        public void Update_end_bidding(int? id_bid)
        {
            var update_bidding = db.update_bidding.Where(s=>s.Bid_id==id_bid);
            int soluong = update_bidding.ToList().Count();
            var bidding = db.bids.Find(id_bid);
            
            int id_artwork = bidding.Art_id;
            var artwork = db.artworks.Find(id_artwork);
            
            if (soluong != 0)
            {
                var danhsach = db.update_bidding
                  .OrderByDescending(s => s.Amount).ToList()
                  .Where(s => s.Bid_id.Equals(id_bid));

                var moneyend = danhsach.FirstOrDefault().Amount;

                bidding.End_Price = double.Parse(moneyend);
                bidding.Status = 1;
                artwork.status = 1;

            }
            else
            {
             bidding.Status = 3;
              
                artwork.status = 0;
            }
            db.Entry(bidding).State = EntityState.Modified;
            db.Entry(artwork).State = EntityState.Modified;
            db.SaveChanges();   
            
          
        }

        public int TimeBid(int? id_bid)
        {
            DateTime time_now = DateTime.Now;
            bid dbtime = db.bids.Find(id_bid);
           DateTime endtime = DateTime.Parse(dbtime.Date_end);
            

            int result = DateTime.Compare(time_now,endtime);
            return result;
        }
        
        public ActionResult ThanhToan(int total,int cus_id,int bid_id)
        {
            Session["bid_id"] = bid_id ;
            Session["cus_order"] = cus_id;
            Session["total"] = total;
            ViewBag.Payment_id = new SelectList(db.payment_method, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThanhToan([Bind(Exclude = "Date_start,Cus_id")] order_buy order)
        {
            int cus_id = int.Parse(Session["cus_order"].ToString());
            int bid_id = int.Parse(Session["bid_id"].ToString());
            DateTime date_start = DateTime.Now;
            if (!ModelState.IsValid)
            {
                ViewBag.Payment_id = new SelectList(db.payment_method, "Id", "Name");

                return View();
            }
            var city = order.City.Trim();
            order.City = city;
            var phone = order.PhoneNumber.Trim();
            order.PhoneNumber = phone;
            var zipcode = order.Zip_code.Trim();
            order.Zip_code = zipcode;
            var country_code = order.Country_code.Trim();
            order.Country_code = country_code;

            order.Cus_id = cus_id;
            order.Date_start = date_start.ToString();
            order.status = 0;

            db.order_buy.Add(order);

            var bids = db.bids.Find(bid_id);
            bids.Status = 4;
            db.Entry(bids).State = EntityState.Modified;
            db.SaveChanges();

         
            return RedirectToAction("Success");

        }
        public ActionResult Success()
        {
            return View();
        }

    }
}
