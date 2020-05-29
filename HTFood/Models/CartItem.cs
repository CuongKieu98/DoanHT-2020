using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTFood.Models
{
    public class CartItem
    {
        
        public int SanPhamID { get; set; }
        public string TenSanPham { get; set; }
        public int DonGia { get; set; }
        public int SoLuong { get; set; }
        public int ThanhTien
        {
            get
            {
                return SoLuong * DonGia;
            }
        }

    }
}