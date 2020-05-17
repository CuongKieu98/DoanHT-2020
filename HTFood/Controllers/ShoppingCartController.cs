using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTFood.Models;

namespace HTFood.Controllers
{
    public class ShoppingCartController : Controller
    {
       
        private dbHutechfoodContext db = new dbHutechfoodContext();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            return View();
        }
        public List<Item> Laygiohang()
        {
            List<Item> lstGiohang = Session["Giohang"] as List<Item>;
            if (lstGiohang == null)
            {
                //Neu gio hang chua ton tai thi khoi tao listGiohang
                lstGiohang = new List<Item>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }
        public ActionResult ThemGiohang(int MaDA, string strURL)
        {
            //Lay ra Session gio hang
            List<Item> lstGiohang = Laygiohang();
            //Kiem tra sách này tồn tại trong Session["Giohang"] chưa?
            Item doan = lstGiohang.Find(n => n.iMaDA == MaDA);
            if (doan == null)
            {
                doan = new Item(MaDA);
                lstGiohang.Add(doan);
                return Redirect(strURL);
            }
            else
            {
               
               
            }
            return View("Index");
        }
        //public ActionResult OrderNow(int? id)
        //{
        //    if(Session[strCart] == null)
        //    {
        //        List<Item> cart = new List<Item>
        //        {
        //            new Item(db.DoAns.Find(id),1)
        //        };

        //        Session[strCart] = cart;

        //    }
        //    else
        //    {
        //        List<Item> cart = (List<Item>)Session[strCart];
        //        cart.Add(new Item(db.DoAns.Find(id), 1));
        //        Session[strCart] = cart;



        //    }
        //    return View("Cart");
        //}
    }
}