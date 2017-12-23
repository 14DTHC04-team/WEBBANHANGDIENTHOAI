using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanHangDienThoai.Models;

namespace WebBanHangDienThoai.ViewModel
{
    public class ItemGioHang
    {
        public string TenSP { get; set; }
        public int MaSP { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
        public string HinhAnh { get; set; }
        public ItemGioHang(int MaSP,int sl)
        {
            using (QLBanHangDienThoaiEntities db = new QLBanHangDienThoaiEntities())
            {
                this.MaSP = MaSP;
                SanPham sp = db.SanPhams.Single(n => n.MaSP == MaSP);
                TenSP = sp.TenSP;
                DonGia = sp.DonGia.Value;
                HinhAnh = sp.HinhAnh;
                SoLuong = sl;
                ThanhTien = DonGia * SoLuong;
            }
        }
        public ItemGioHang(int MaSP)
        {
            using (QLBanHangDienThoaiEntities db = new QLBanHangDienThoaiEntities())
            {
                this.MaSP = MaSP;
                SanPham sp = db.SanPhams.Single(n => n.MaSP == MaSP);
                TenSP = sp.TenSP;
                DonGia = sp.DonGia.Value;
                HinhAnh = sp.HinhAnh;
                SoLuong = 1;
                ThanhTien = DonGia * SoLuong;
            }
        }
        public ItemGioHang()
        {

        }
    }
}