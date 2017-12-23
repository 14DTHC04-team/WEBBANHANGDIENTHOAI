using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangDienThoai.Models;

namespace WebBanHangDienThoai.Controllers
{
    public class SearchController : Controller
    {//Hai loz
        QLBanHangDienThoaiEntities db = new QLBanHangDienThoaiEntities();
        // GET: Search
        public ActionResult KQTimKiem(string tukhoa)
        {
            //tìm kiếm tên sản phẩm
            var listsp = (db.SanPhams.Where(n => n.TenSP.Contains(tukhoa)).OrderBy(n => n.TenSP));
            return View(listsp);
            
        }
        public ActionResult TKGia(int min,int max)
        {
            List<SanPham> listsp = new List<SanPham>();
            
           
            return View(listsp);
        }
    }
}