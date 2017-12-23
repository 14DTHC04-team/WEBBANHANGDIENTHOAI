using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangDienThoai.Models;

namespace WebBanHangDienThoai.Controllers
{
    public class HomeController : Controller
    {
        QLBanHangDienThoaiEntities db = new QLBanHangDienThoaiEntities();
        // GET: Home
        public ActionResult Index()
        {
            //List Dien thoai moi
            var lstDTM = db.SanPhams.Where(n => n.MaLoaiSP == 1 );
            ViewBag.ListDTM = lstDTM;

            //List LapTop moi
            var lstLTM = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == 1 && n.DaXoa == 0);
            ViewBag.ListLTM = lstLTM;
            return View();
        }
        public ActionResult MenuPartial()
        {
            var lstSP = db.SanPhams;
            return PartialView(lstSP);
        }
    }
}