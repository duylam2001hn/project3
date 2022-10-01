using Gallery_art_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using common;
using System.Data.Entity;
using System.Dynamic;

namespace Gallery_art_3.Controllers
{
    public class CartController : Controller
    {
        private const string CartSession = "CartSession";
        private const string CusSession = "idUser";
        private Datacontext db = new Datacontext();
        // GET: Sale

        public ActionResult Index()
        {
            if (Session[CusSession] == null)
            {
                return RedirectToAction("Login", "customers");
            }
            List<CartItem> giohang = Session[CartSession] as List<CartItem>;
            return View(giohang);
            
        }


        public RedirectToRouteResult ThemVaoGio(int SanPhamID)
        {
            if (Session[CusSession] == null)
            {
                return RedirectToAction("Login", "customers");
            }

            if (Session[CartSession] == null) // Nếu giỏ hàng chưa được khởi tạo
            {
                Session[CartSession] = new List<CartItem>();  // Khởi tạo Session["giohang"] là 1 List<CartItem>
            }

            List<CartItem> giohang = Session[CartSession] as List<CartItem>;  // Gán qua biến giohang dễ code

            // Kiểm tra xem sản phẩm khách đang chọn đã có trong giỏ hàng chưa

            var check_item = giohang.Where(s => s.SanPhamID.Equals(SanPhamID)).Count();
            if (check_item ==0) // ko co sp nay trong gio hang
            {
                
                artwork sp = db.artworks.Find(SanPhamID);  // tim sp theo sanPhamID

                CartItem newItem = new CartItem()
                {
                    SanPhamID = SanPhamID,
                    TenSanPham = sp.Title,
                    SoLuong = 1,
                    Hinh = sp.img_path,
                    DonGia = (int)sp.Price

                };  // Tạo ra 1 CartItem mới

                giohang.Add(newItem);  // Thêm CartItem vào giỏ 
            }
    
            // Action này sẽ chuyển hướng về trang chi tiết sp khi khách hàng đặt vào giỏ thành công. Bạn có thể chuyển về chính trang khách hàng vừa đứng bằng lệnh return Redirect(Request.UrlReferrer.ToString()); nếu muốn.
            return RedirectToAction("Details", "artworks", new { id = SanPhamID });
        }



        public RedirectToRouteResult XoaKhoiGio(int SanPhamID)
        {
            List<CartItem> giohang = Session[CartSession] as List<CartItem>;
            CartItem itemXoa = giohang.FirstOrDefault(m => m.SanPhamID == SanPhamID);
            if (itemXoa != null)
            {
                giohang.Remove(itemXoa);
            }
            return RedirectToAction("Index");
        }

        public ActionResult ThanhToan(int total)
        {
            //var giohang = Session[CartSession] as List<CartItem>;
            Session["total"] = total;
            ViewBag.Payment_id = new SelectList(db.payment_method, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThanhToan([Bind(Exclude = "Date_start,Cus_id")] order_buy order)
        {
            
           
            int cus_id = int.Parse(Session[CusSession].ToString());

            DateTime date_start = DateTime.Now;
            var cus = db.customers.Find(cus_id);
           
           
           
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

            //order_buy order = new order_buy();
            order.Cus_id = cus_id;
                order.Date_start = date_start.ToString();
                order.status = 0;
                
                db.order_buy.Add(order);
                db.SaveChanges();
           
          
            
            var order_buy = db.order_buy.ToList();
            var new_order = order_buy.LastOrDefault(s => s.Cus_id == cus_id);
            var cart = Session[CartSession] as List<CartItem>;
            foreach(var item in cart)
            {
                order_detail order_Detail = new order_detail();
                order_Detail.Order_id = new_order.Id;
                order_Detail.Art_id = item.SanPhamID;
                order_Detail.Quantity = item.SoLuong;
                db.order_detail.Add(order_Detail);
                changeStatusArtwork(item.SanPhamID);
                db.SaveChanges();

            }

            Session[CartSession] = "";


            //var email = cus.Email;
            //var content = "Bạn có 1 đơn hàng mới từ Gallery Art shop";
           
            //new MailHelper().SendMail(email, "Đơn hàng mới từ Gallery Art",content);
            return RedirectToAction("Success");
            
        }

        public ActionResult Success()
        {
            return View();
        }

        public void changeStatusArtwork(int id_artwork)
        {
            var artwork = db.artworks.Find(id_artwork);
            artwork.status = 1;
            db.Entry(artwork).State = EntityState.Modified;
            db.SaveChanges();
        }

    }
}