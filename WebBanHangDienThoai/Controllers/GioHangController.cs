using System.Collections.Generic;
using System.Web.Mvc;
using WebBanHangDienThoai.ViewModel;
using WebBanHangDienThoai.Models;
using System.Linq;
using System;

namespace WebBanHangDienThoai.Controllers
{
    public class GioHangController : Controller
    {
        QLBanHangDienThoaiEntities db = new QLBanHangDienThoaiEntities();
      
        public ActionResult GioHangPartial()
        {
            if(TinhTongSoLuong()==0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }
        //Lay Gio Hang
        public List<ItemGioHang> LayGioHang()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            //nếu session giỏ hàng chưa tồn tại thì khởi tạo
            if(lstGioHang==null)
            {
                lstGioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstGioHang;          
            }
            return lstGioHang;
        }
        //Them Gio Hang
       public ActionResult ThemGioHang(int MaSP,string strURL)
        {
            //Kiem Tra san Pham co ton tai trong csdl khong
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if(sp==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lay gio hang
            List<ItemGioHang> lstGioHang = LayGioHang();
            //TH 1 sản phẩm đã tồn tại trong giỏ hàng
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if(spCheck!=null)
            {
                //if(sp.SoLuongTon<spCheck.SoLuong)
                //{
                //    return View("ThongBao");
                //}
                spCheck.SoLuong++;
                return Redirect(strURL);
            }
            ItemGioHang itemGioHang = new ItemGioHang(MaSP);
            lstGioHang.Add(itemGioHang);
            return Redirect(strURL);

        }
        public double TinhTongSoLuong()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if(lstGioHang==null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.SoLuong);

        }
        public decimal TinhTongTien()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.ThanhTien);
        }

        //[HttpPost]
        //public JsonResult AddToCart(int id)
        //{
        //    List<ItemGioHang> listCartItem;
        //    if(Session["ShoppingCart"]==null)
        //    {
        //        listCartItem = new List<ItemGioHang>();
        //        listCartItem.Add(new ItemGioHang {SoLuong=1, });
        //    }
        //}
        //Thêm Giỏ hàng Ajax
        public ActionResult ThemGioHangAjax(int MaSP)
        {
            //Kiem Tra san Pham co ton tai trong csdl khong
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lay gio hang
            List<ItemGioHang> lstGioHang = LayGioHang();
            //TH 1 sản phẩm đã tồn tại trong giỏ hàng
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck != null)
            {
                //if(sp.SoLuongTon<spCheck.SoLuong)
                //{
                //    return View("ThongBao");
                //}
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                ViewBag.TongSoLuong = TinhTongSoLuong();
                ViewBag.TongTien = TinhTongTien();
                return PartialView("GioHangPartial");
            }
            ItemGioHang itemGioHang = new ItemGioHang(MaSP);
            lstGioHang.Add(itemGioHang);
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView("GioHangPartial");

        }
        // GET: GioHang
        public ActionResult XemGioHang()
        {
            List<ItemGioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }
        //Chỉnh sửa giỏ hàng
        [HttpGet]
        public ActionResult SuaGioHang(int MaSP)
        {
            //Kiểm tra session giỏ hàng tồn tại chưa
            if (Session["GioHang"]==null)
            {
                return RedirectToAction("Index","Home");
            }
            //Kiểm tra sản phẩm có tồn tại trong csdl hay không
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if(sp==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy list giỏ hàng từ session
            List<ItemGioHang> lstGioHang = LayGioHang();
            //Kiểm tra sản phẩm có tồn tại trong giỏ hàng hay không
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if(spCheck==null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.GioHang = lstGioHang;

            //Đã tồn tại
            return View(spCheck);
        }
        [HttpPost]
        public ActionResult CapNhatGioHang(ItemGioHang itemGH)
        {
            //Kiểm tra 
            List<ItemGioHang> lstGH = LayGioHang();
            ItemGioHang itemGHUpdate = lstGH.Find(n => n.MaSP == itemGH.MaSP);
            itemGHUpdate.SoLuong = itemGH.SoLuong;
            itemGHUpdate.ThanhTien = itemGHUpdate.SoLuong * itemGHUpdate.DonGia;
            return RedirectToAction("XemGioHang");
        }
        public ActionResult XoaGioHang(int MaSP)
        {    
            //Kiểm tra session giỏ hàng tồn tại chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Kiểm tra sản phẩm có tồn tại trong csdl hay không
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy list giỏ hàng từ session
            List<ItemGioHang> lstGioHang = LayGioHang();
            //Kiểm tra sản phẩm có tồn tại trong giỏ hàng hay không
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Xóa item trong giỏ hàng
            lstGioHang.Remove(spCheck);
            return RedirectToAction("XemGioHang");
        }
        public ActionResult DatHang(KhachHang kh)
        {
       
            //Kiểm tra session giỏ hàng tồn tại chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
         
            KhachHang khachHang = new KhachHang();
            //Đổi với khác hàng không đăng nhập
            if (Session["TaiKhoan"]==null)
            {
                khachHang = kh;
                db.KhachHangs.Add(khachHang);
                db.SaveChanges();
            }
            //Đổi với khác hàng đã đăng nhập
            else
            {
                AspNetUser thanhVien = Session["TaiKhoan"] as AspNetUser;
                khachHang.Email = thanhVien.Email;
                khachHang.TenKH = thanhVien.Name;
                khachHang.DiaChi = thanhVien.Address;
                khachHang.SoDienThoai = thanhVien.PhoneNumber;
                db.KhachHangs.Add(khachHang);
                db.SaveChanges();

            }
            //Thêm đơn hàng
            DonDatHang ddh = new DonDatHang();
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            ddh.IdKhachHang = khachHang.MaKH;
            db.DonDatHangs.Add(ddh);
            db.SaveChanges();
            //Thêm chi tiết đơn đặt hàng
            List<ItemGioHang> lstGH = LayGioHang();
            foreach(var item in lstGH)
            {
                ChiTietDonDatHang ctdh = new ChiTietDonDatHang();
                ctdh.MaDDH = ddh.MaDDH;
                ctdh.MaSP = item.MaSP;
                ctdh.TenSP = item.TenSP;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = item.DonGia;
                db.ChiTietDonDatHangs.Add(ctdh);
           
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang");
        }
    }
        
}