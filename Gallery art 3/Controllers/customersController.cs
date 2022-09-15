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

namespace Gallery_art_3.Controllers
{
    public class customersController : Controller
    {
        private Datacontext db = new Datacontext();

        // GET: customers
        public ActionResult Index()
        {
            if (Session["idUser"] != null)
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
            id = int.Parse(Session["idUser"].ToString());
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
        public ActionResult Register([Bind(Include = "Id,FullName,Address,Email,Password,City")] customer customer)
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
                            customer.Password = passwordHash(Request["Password"]);
                            db.customers.Add(customer);
                            db.SaveChanges();

                        if (job == 0)
                        {
                            //Sau sửa phần này điều hướng về trang chủ Home
                            ViewBag.Message = "Register Successfully";
                            return View("Register");
                        }
                        return RedirectToAction("becomeartist");
                        //return RedirectToAction("Create","artistsController");
                        
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
                    Session["idUser"] = datacus.FirstOrDefault().Id;
                    int user_id = Int32.Parse(Session["idUser"].ToString());
                    if(datacus.FirstOrDefault().artists.Count() > 0)
                    {
                        //from t in db.artists where t.Id == user_id
                        var dataArtist = db.artists.Where(s=>s.Cus_id.Equals(user_id)) ; 
                        Session["idArtist"] = dataArtist.FirstOrDefault().Id;
                        return RedirectToAction("Details","artists");
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
        public ActionResult Edit([Bind(Include = "FullName,Address,Email,Password,City")] customer customer)
        {
           if (ModelState.IsValid)
                {
                  db.Entry(customer).State = EntityState.Modified;
                  db.SaveChanges();
                  return RedirectToAction("Index");
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
            int id_cus = db.customers.ToList().Last().Id;
            if (Session["idUser"] != null)
            {
                id_cus = int.Parse(Session["idUser"].ToString());
            }
            
          
            DateTime now = DateTime.Now ;

            int month = int.Parse(Request["expire"].ToString());


            DateTime expire = now.AddMonths(month);
                Session["idUser"] = id_cus;
                Session["ExpireDate"] = expire;

                
           
            return RedirectToAction("Create", "artists");


        }



        [HttpGet]
        public ActionResult ChangePassword(int? id)
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
            return View("ChangePassword");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword([Bind(Include = "Password")] customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

    }
    
}
