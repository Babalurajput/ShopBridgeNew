using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopBridge.Models;
using System.IO;

namespace ShopBridge.Controllers
{
    public class SalesController : Controller
    {
        private ShopBridgeContext db = new ShopBridgeContext();
        IItemRepository _repository;

        public SalesController() : this(new ItemRepository()) { }

        public SalesController(IItemRepository repository)
        {
            _repository = repository;
        }
        public ViewResult Index()
        {
            ViewData["ControllerName"] = this.ToString();
            return View("Index", _repository.GetAllItem());
        }

        // GET: Sales/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = await db.Sales.FindAsync(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // GET: Sales/Create
        // GET: Sales/Create
        [HttpGet]
        public ActionResult CreateNew()
        {
            Sale sale = new Sale();
            sale.Image = null;
            return View(sale);
        }

        // POST: Sales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Sale sale, HttpPostedFileBase file)
        {
            try
            {
                string strmsg = "";
                strmsg = FormValidation(sale);
                if (strmsg == "")
                {
                    if (ModelState.IsValid)
                    {
                        if (file != null)
                        {
                            byte[] bytes;
                            using (System.IO.BinaryReader br = new BinaryReader(file.InputStream))
                            {
                                bytes = br.ReadBytes(file.ContentLength);
                            }
                            sale.Image = bytes;
                        }
                        db.Sales.Add(sale);
                        _repository.CreateNewItem(sale);
                        return RedirectToAction("Index");
                    }
                }
                if (file != null)
                {
                    byte[] bytes;
                    using (BinaryReader br = new BinaryReader(file.InputStream))
                    {
                        bytes = br.ReadBytes(file.ContentLength);
                    }
                    sale.Image = bytes;
                }
                ViewBag.strmsg = strmsg;
                return View("CreateNew", sale);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex);
                ViewData["CreateError"] = "Unable to create; view innerexception";
            }
            return View("CreateNew");
        }
        public string FormValidation(Sale sale)
        {
            string strError = "";
            if (String.IsNullOrWhiteSpace(sale.Name) == true)
            {
                return strError = "Please Enter Name";
            }
            if (String.IsNullOrWhiteSpace(sale.Description) == true)
            {
                return strError = "Please Enter Description";
            }
            if (sale.Price.Equals(null))
            {
                return strError = "Please Enter Price";
            }
            return strError;
        }

        // GET: Sales/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = await db.Sales.FindAsync(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Sale sale, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string strmsg = "";
                    strmsg = FormValidation(sale);
                    if (strmsg == "")
                    {
                        using (ShopBridgeContext db = new ShopBridgeContext())
                        {
                            var ID = db.Sales.Find(sale.Id);
                            ID.Name = sale.Name;
                            ID.Description = sale.Description;
                            ID.Price = sale.Price;
                            if (file != null)
                            {
                                byte[] bytes;
                                using (BinaryReader br = new BinaryReader(file.InputStream))
                                {
                                    bytes = br.ReadBytes(file.ContentLength);
                                }
                                ID.Image = bytes;
                            }
                            db.SaveChanges();
                        }
                        return RedirectToAction("Index");
                    }
                    if (file != null)
                    {
                        byte[] bytes;
                        using (BinaryReader br = new BinaryReader(file.InputStream))
                        {
                            bytes = br.ReadBytes(file.ContentLength);
                        }
                        sale.Image = bytes;
                    }
                    ViewBag.strmsg = strmsg;
                    return View("Edit",sale);
                }
                catch (Exception ex) { ModelState.AddModelError("", "error message"); }
                finally { }
            }
            return View(sale);
        }

        // GET: Sales/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = await db.Sales.FindAsync(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Sale sale = await db.Sales.FindAsync(id);
            db.Sales.Remove(sale);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
