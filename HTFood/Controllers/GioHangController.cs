using HTFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HTFood.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        public ActionResult Index()
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            return View(giohang);
          
        }
    }
}