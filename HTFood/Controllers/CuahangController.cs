﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HTFood.Models;
using Newtonsoft.Json;
using PagedList;

namespace HTFood.Controllers
{
    public class CuahangController : Controller
    {
        string url = Constants.url;
        HttpClient client;
        public static List<Cuahang> listch = new List<Cuahang>();
        public CuahangController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/jason"));
        }
        private dbHutechfoodContext db = new dbHutechfoodContext();

        // GET: Cuahang
        public async Task<ActionResult> Index()
        {
            HttpResponseMessage responseMessage = await client.GetAsync(url + @"cuahang/");
            List<Cuahang> ch = getAllCuaHang(responseMessage);
            if (ch != null)
            {
                ViewBag.accept = false;
                var list = ch.ToList();
                return View(list);
            }
            return View("Error");
        }

        public static List<Cuahang> getAllCuaHang(HttpResponseMessage responseMessage)
        {
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                List<Cuahang> cuahangs = JsonConvert.DeserializeObject<List<Cuahang>>(responseData, settings);
                var listch = cuahangs.ToList();
                return listch;
            }
            return null;
        }
        // GET: Cuahang/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            Cuahang cuahangs = null;
            HttpResponseMessage response = await client.GetAsync(url + @"Cuahang/" + id);
            if (response.IsSuccessStatusCode)
            {
                cuahangs = await response.Content.ReadAsAsync<Cuahang>();
                //dong hoac mo data table 
                ViewBag.accept = false;

                //lấy danh mục cửa hàng=>>
                // Call api
                HttpResponseMessage responseMessage = await client.GetAsync(url + @"DanhMucDoAn/");
                // Get all data
                List<DanhMucDoAn> danhmucdoans = DanhMucDoAnController.getAllDanhMuc(responseMessage);
                // Check data 
                danhmucdoans = danhmucdoans.Where(n => n.MaCH == id).ToList();
                ViewBag.danhmuc = danhmucdoans;
                ViewBag.CountMenu = danhmucdoans.Count;
                responseMessage = await client.GetAsync(url + @"Doan");
                List<DoAn> listda = DoAnController.getAllDoAn(responseMessage);
                listda = listda.Where(n => n.MaDM == id).ToList();
                ViewBag.doan = listda;
                ViewBag.CountDoan = listda.Count;
                //lấy khuyến mãi của cửa hàng ->>>

                responseMessage = await client.GetAsync(url + @"Khuyenmai/");             
                List<Khuyenmai> listkm = KhuyenmaiController.getAllKhuyenmai(responseMessage);
                listkm = listkm.Where(n => n.MaCH == id).ToList();
                ViewBag.sale = listkm;      
                ViewBag.CountSale = listkm.Count;
                //don hang
                responseMessage = await client.GetAsync(url + @"DonDatHang/");
                List<DonDatHang> listddh = DonDatHangController.getAllOrder(responseMessage);
                listddh = listddh.Where(n => n.MaKH == id).ToList();
                ViewBag.Donhang = listddh;
                ViewBag.CountOrders = listddh.Count;
                //sasa




            }
            return View(cuahangs);

        }
     

        // POST: Cuahang/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cuahang cuahangs)
        {
            HttpResponseMessage response = client.PostAsJsonAsync(url + @"cuahang/", cuahangs).Result;
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                ViewBag.Detail = "Sucess";
            }
            return RedirectToAction("Index");
        }

        // GET: Cuahang/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            Cuahang cuahangs = null;
            HttpResponseMessage response = await client.GetAsync(url + @"cuahang/" + id);
            if (response.IsSuccessStatusCode)
            {
                cuahangs = await response.Content.ReadAsAsync<Cuahang>();
            }
            return View(cuahangs);
        }

        // POST: Cuahang/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Cuahang cuahangs)
        {
            HttpResponseMessage response = client.PutAsJsonAsync(url + @"cuahang/" + cuahangs.MaCH, cuahangs).Result;
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Index");
        }

        // GET: Cuahang/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            HttpResponseMessage response = await client.DeleteAsync(url + @"cuahang/" + id);
            return RedirectToAction("Index", "CuaHang");
        }
        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "success")
            {
                TempData["AlertType"] = "alert bg-green";
            }
            else if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";

            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
