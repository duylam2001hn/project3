using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gallery_art_3.Models;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using common;
using PagedList;

namespace Gallery_art_3.Controllers
{
    public class customersController : Controller
    {
        private const string CusSession = "idUser";
        private Datacontext db = new Datacontext();

        // GET: customers
        public ActionResult Index()
        {
            if (Session[CusSession] != null)
            {
                return View(db.customers.ToList());
            }
            return View("Login");
        }

        // GET: customers/Details/5
        public ActionResult Details(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            id = int.Parse(Session[CusSession].ToString());
            customer customer = db.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: customers/Create
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // POST: customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Id,FullName,Email,Password")] customer customer)
        {
            string password = Request["Password"].ToString();
            int job = Int32.Parse(Request["job"].ToString());
            
            if (ModelState.IsValid)
            {
                    var check = db.customers.FirstOrDefault(s=>s.Email == customer.Email);
                    if (check == null)
                    {
                    if (confirmPassword(password) == 1)
                    {
                            customer.Status = 0;
                            customer.Password = passwordHash(Request["Password"]);
                            db.customers.Add(customer);
                            db.SaveChanges();

                        if (job == 0)
                        {
                            //Sau sửa phần này điều hướng về trang chủ Home
                            ViewBag.Message = "Register Successfully";
                            return View();
                        }
                        return RedirectToAction("becomeartist");
                        
                        
                    }
                    ViewBag.Message = "The confirm password does not match";
                    return View();
                    }
                else
                {
                    ViewBag.Message = "Email already exists";
                    return View();
                }
                
            }
            
            return View();
        }

        

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var f_password = passwordHash(password);
                var datacus = db.customers.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password)).ToList();
               
                if (datacus.Count() > 0)
                {
                    //add session
                    Session["FullName"] = datacus.FirstOrDefault().FullName;
                    Session["Email"] = datacus.FirstOrDefault().Email;
                    Session[CusSession] = datacus.FirstOrDefault().Id;
                    int user_id = int.Parse(Session[CusSession].ToString());
                    if(datacus.FirstOrDefault().artists.Count() > 0)
                    {
                        //from t in db.artists where t.Id == user_id
                        var dataArtist = db.artists.Where(s=>s.Cus_id.Equals(user_id)) ;

                        int artist_id = dataArtist.FirstOrDefault().Id;
                        int result = Time_end_artist(artist_id);
                        if (result < 0)
                        {
                            Session["idArtist"] = dataArtist.FirstOrDefault().Id;
                            return RedirectToAction("Details", "artists");

                        }

                       

                        return RedirectToAction("Details");
                    }
                    return RedirectToAction("Details");
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }

        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }


        // GET: customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customer customer = db.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FullName,Email")] customer customer)
        {
           if (ModelState.IsValid)
                {
                  db.Entry(customer).State = EntityState.Modified;
                  db.SaveChanges();
                  return RedirectToAction("Details");
                }
            return View(customer);
        }

        // GET: customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            customer customer = db.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            customer customer = db.customers.Find(id);
            db.customers.Remove(customer);
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


        public int confirmPassword(string passsword)
        {
            string confirmpass = Request["ConfirmPass"].ToString();

            if (confirmpass.Equals(passsword))
            {
                return 1;
            }
            return 0;
        }

        public string passwordHash(string password)
        {
            
            MD5 mh = MD5.Create();
            //Chuyển kiểu chuổi thành kiểu byte
            byte[] inputBytes = Encoding.ASCII.GetBytes(password);
            //mã hóa chuỗi đã chuyển
            byte[] hash = mh.ComputeHash(inputBytes);
            //tạo đối tượng StringBuilder (làm việc với kiểu dữ liệu lớn)
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            string passwordhash = sb.ToString();

            return passwordhash;
        }

        public ActionResult becomeartist()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult becomeartist(string artist)
        {
            
            if (Session[CusSession] == null)
            {
                int id_cus = db.customers.ToList().Last().Id;
                Session[CusSession] = id_cus;
            }

            int idcus = int.Parse(Session[CusSession].ToString());

            var artist_exist = db.artists.Where(s => s.Cus_id.Equals(idcus)) ;
            int count = artist_exist.Count();

            var artist_art = db.artists.Find(artist_exist.FirstOrDefault().Id);
            
          
            DateTime now = DateTime.Now ;

            int month = int.Parse(Request["expire"].ToString());


            DateTime expire = now.AddMonths(month);
                
                Session["ExpireDate"] = expire;

            if (Session["idArtist"] != null)
            {
                int idArtist = int.Parse(Session["idArtist"].ToString()) ;
                int result = Time_end_artist(idArtist);
                var artist_expire = db.artists.Find(idArtist);
                if (result < 0)
                {
                    DateTime expire_date_hientai = DateTime.Parse(artist_expire.Expire_date);
                    string expire_new = expire_date_hientai.AddMonths(month).ToString();
                    artist_expire.Expire_date = expire_new;
                    db.Entry(artist_expire).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", "artists");
                }
            }else if(count == 1)
            {
                artist_art.Expire_date = expire.ToString();
                db.Entry(artist_art).State = EntityState.Modified;
                db.SaveChanges();

            }
           
            return RedirectToAction("Logout");


        }

        public int Time_end_artist(int? id_artist)
        {
            DateTime time_now = DateTime.Now;
            artist dbtime = db.artists.Find(id_artist);
            DateTime endtime = DateTime.Parse(dbtime.Expire_date);


            int result = DateTime.Compare(time_now, endtime);
            return result;
        }


        //[HttpGet]
        //public ActionResult ChangePassword(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    customer customer = db.customers.Find(id);
        //    if (customer == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View("ChangePassword");
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ChangePassword([Bind(Include = "Password")] customer customer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(customer).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(customer);
        //}

        //}



        //forgot password
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordView customer)
        {
            if (ModelState.IsValid)
            {
               

                var cus = db.customers.Where(s => s.Email.Equals(customer.Email));


                int exist_cus = cus.Count();
                

                if (exist_cus == 0)
                {
                    return View("ForgotPasswordConfirmation");
                }
                DateTime Link_recieve = DateTime.Now;
                DateTime expire = Link_recieve.AddMinutes(2);

                string expire_link  = expire.ToString();

              
                var email_to = customer.Email;
              
                var callbackUrl = Url.Action("ResetPassword","customers", new { time = expire_link }, protocol: Request.Url.Scheme);
                string content = "From Gallery Art . Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>";
                
                new MailHelper().SendMail(email_to,"Reset Password từ Gallery Art",content);
             
                return RedirectToAction("ForgotPasswordConfirmation");
               

            }
            return View(customer);
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }


        //Reset Password
        [AllowAnonymous]
        public ActionResult ResetPassword(string time)
        {
            DateTime now = DateTime.Now;
            DateTime expire = DateTime.Parse(time);

            if(expire < now)
            {
                return View("ForgotPassword");
            }
           
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordView customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }
            var cus = db.customers.Where(s=>s.Email.Equals(customer.Email));
            if (cus == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            int cus_id = cus.FirstOrDefault().Id;
            var cus_new_pass = db.customers.Find(cus_id);
            var new_pass_hash = passwordHash(customer.Password);
            cus_new_pass.Password = new_pass_hash;
            db.Entry(cus_new_pass).State = EntityState.Modified;
            if (customer.ConfirmPassword == customer.Password)
            {
                db.SaveChanges();
                return RedirectToAction("ResetPasswordConfirmation");
            }
            
            return View();
        }


        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        public ActionResult Add_Favorite_art(int id_artwork)
        {

            if (Session[CusSession] == null)
            {
                return RedirectToAction("Login");
            }
            int id_cus = int.Parse(Session[CusSession].ToString());
            var check = db.favorite_artwork.Where(
                fa => fa.Artwork_id==id_artwork && fa.Cus_id == id_cus).Count();
            favorite_artwork favorite_Artwork = new favorite_artwork();
            favorite_Artwork.Artwork_id = id_artwork;
            favorite_Artwork.Cus_id = id_cus;
            db.favorite_artwork.Add(favorite_Artwork);

            if (check != 1)
            {
                db.SaveChanges();
            }

            return RedirectToAction("Artwork", "TestHome");
          
           
        }

        public ActionResult Favorite_art()
        {
            int id_cus = int.Parse(Session[CusSession].ToString());
            var favorite_art = db.favorite_artwork.ToList().Where(s=>s.Cus_id == id_cus);
            return View(favorite_art);
        }

        public ActionResult purchase_history(int? page)
        {

            if (page == null) page = 1;

          
            int pageSize = 4;

            int pageNumber = (page ?? 1);
            int id_cus = int.Parse(Session[CusSession].ToString());
            var purchase_history = db.order_detail.Where(s => s.order_buy.Cus_id == id_cus).OrderByDescending(s=>s.order_buy.Date_start);
            return View(purchase_history.ToPagedList(pageNumber, pageSize));
        }


    }
    
}
