using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBanHangDienThoai.Models;

namespace WebBanHangDienThoai.Controllers
{
    [Authorize(Roles ="Admin")]
    public class QuanLySanPhamController : Controller
    {
        // GET: QuanLySanPham
        QLBanHangDienThoaiEntities db = new QLBanHangDienThoaiEntities();

        public ActionResult Index()
        {
            return View(db.SanPhams.Where(n=>n.DaXoa==0));
        }

        [HttpGet]
        public ActionResult Taomoi()
        {
            //DropListDown cho NCC,NSX,LoaiSP
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");

            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Taomoi(SanPham sp,HttpPostedFileBase[] HinhAnh)
        {   //DropListDown cho NCC,NSX,LoaiSP
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");
            if (HinhAnh[0].ContentLength > 0)
            {   
                //Lấy tên hình ảnh
                var fileName = Path.GetFileName(HinhAnh[0].FileName);
                //Lấy hình ảnh chuyển vào thư mục hình ảnh
                var path = Path.Combine(Server.MapPath("~/Content/images"),fileName);
                HinhAnh[0].SaveAs(path);
                sp.HinhAnh = fileName;
            }
            db.SanPhams.Add(sp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        
        [HttpGet]
        public ActionResult ChinhSua(int? id)
        {
            //lấy sản phẩm cần chỉnh sửa dựa vào id
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }

            
            //DropListDown cho NCC,NSX,LoaiSP
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC",sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai",sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX",sp.MaNSX);
            return View(sp);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(SanPham model, HttpPostedFileBase[] HinhAnh)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC",model.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai",model.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX",model.MaNSX);
            
            if (HinhAnh[0] != null)
            {
                //Lấy tên hình ảnh
                var fileName = Path.GetFileName(HinhAnh[0].FileName);
                //Lấy hình ảnh chuyển vào thư mục hình ảnh
                var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                HinhAnh[0].SaveAs(path);
                model.HinhAnh = fileName;
                model.HinhAnh1 = fileName;
                model.HinhAnh2 = fileName;
                model.HinhAnh3 = fileName;
            }
            else
            {

                model.HinhAnh = "iPhone 6 32GB (2017)_0.jpg";
                model.HinhAnh1 = "iPhone 6 32GB (2017)_0.jpg";
                model.HinhAnh2 = "iPhone 6 32GB (2017)_0.jpg";
                model.HinhAnh3 = "iPhone 6 32GB (2017)_0.jpg";
            }

           
            // Nếu dữ liệu vào chắc chắn ok
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Xoa(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }

            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);
            return View(sp);
        }

        [HttpPost]
        public ActionResult Xoa(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            db.SanPhams.Remove(sp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}