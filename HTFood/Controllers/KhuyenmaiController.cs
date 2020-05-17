using System;
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
    public class KhuyenmaiController : Controller
    {
        string url = Constants.url;
        HttpClient client;
        public static List<Khuyenmai> listKM = new List<Khuyenmai>();
        public KhuyenmaiController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/jason"));
        }
        private dbHutechfoodContext db = new dbHutechfoodContext();

        // GET: Khuyenmai
        public async Task<ActionResult> Index()
        {
            HttpResponseMessage responseMessage = await client.GetAsync(url + @"Khuyenmai/");
            List<Khuyenmai> km = getAllKhuyenmai(responseMessage);
            if (km != null)
            {
                ViewBag.accept = false;
                var list = km.ToList();
                return View(list);
            }
            return View("Error");
        }
        public static List<Khuyenmai> getAllKhuyenmai(HttpResponseMessage responseMessage)
        {
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                List<Khuyenmai> khuyenmais = JsonConvert.DeserializeObject<List<Khuyenmai>>(responseData, settings);
                var listkm = khuyenmais.ToList();
                return listkm;
            }
            return null;
        }
        // GET: Khuyenmai/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            HttpResponseMessage response = await client.GetAsync(url + @"chitietdonhang/" + id);          
            if (response.IsSuccessStatusCode)
            {
                var responseData = response.Content.ReadAsStringAsync().Result;
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                var chitietkms = JsonConvert.DeserializeObject<List<ChiTietKhuyenMai>>(responseData, settings);
                List<ChiTietKhuyenMai> ctkm = chitietkms.ToList();
                ViewBag.MaKM = id;
                
                //lây khuyen mai
                HttpResponseMessage responseMessage = await client.GetAsync(url + @"Khuyenmai/");
                List<Khuyenmai> khuyenmais = getAllKhuyenmai(responseMessage);
                Khuyenmai kms = khuyenmais.SingleOrDefault(n => n.MaKM == id);
                ViewBag.TenKM = kms.TenKM;
                ViewBag.Mota = kms.MotaKM;
                ViewBag.Batdau = kms.TgBatDau;
                ViewBag.KetThuc = kms.TgKetThuc;
              
             
                //lay do an
                responseMessage = await client.GetAsync(url + @"doan/");
                List<DoAn> listda = DoAnController.getAllDoAn(responseMessage);
                List<string> dsTen = new List<string>();
                foreach (ChiTietKhuyenMai ctdh in ctkm)
                {
                    string name = listda.Where(n => n.MaDA == ctdh.MaDA).SingleOrDefault().TenDA;
                    dsTen.Add(name);
                }
                //DoAn doAn = listda.Where(n => n.MaDA == );
                ViewBag.nameDa = dsTen;

                return View(chitietkms.ToList());
            }
            return View();
            
        }

        // GET: Khuyenmai/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Khuyenmai/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Khuyenmai khuyenmai)
        {
            HttpResponseMessage response = client.PostAsJsonAsync(url + @"khuyenmai/", khuyenmai).Result;
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
            
                //
               

                ViewBag.Detail = "Sucess";
            }
            return RedirectToAction("Index");
        }

        // GET: Khuyenmai/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            Khuyenmai khuyenmai = null;
            HttpResponseMessage response = await client.GetAsync(url + @"khuyenmai/" + id);
            if (response.IsSuccessStatusCode)
            {
                khuyenmai = await response.Content.ReadAsAsync<Khuyenmai>();
            }
            return View(khuyenmai);
        }

        // POST: Khuyenmai/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Khuyenmai khuyenmai)
        {
            HttpResponseMessage response = client.PutAsJsonAsync(url + @"nhanviengiaohang/" + khuyenmai.MaKM, khuyenmai).Result;
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Index");
        }

        // GET: Khuyenmai/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            HttpResponseMessage response = await client.DeleteAsync(url + @"khuyenmai/" + id);
            return RedirectToAction("Index", "KhuyenMai");
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
