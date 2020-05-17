using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HTFood.Models;

namespace HTFood.Controllers
{
    
    [Serializable]
    
    public class Item
    {

        dbHutechfoodContext db = new dbHutechfoodContext();
        public int iMaDA { set; get; }
        public string sTenDA { set; get; }

        public int Quantity { get; set; }


   
        public Item()
        {

        }
        public Item(int MaDA)
        {
            iMaDA = MaDA;
            DoAn doan = db.DoAns.Single(n => n.MaDA == iMaDA);
            sTenDA = doan.TenDA;
            Quantity = 1;


        }
    }
}