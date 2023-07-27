using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace projMvcDemo.Controllers
{
    public class ProductController : Controller
    {
        dbDemoEntities db = new dbDemoEntities();
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            tProduct prod = db.tProduct.FirstOrDefault(p => p.fId == id);
            if (prod == null)
                return RedirectToAction("List");
            return View(prod);
        }
        [HttpPost]
        public ActionResult Edit(tProduct pIn)
        {
            tProduct pDb = db.tProduct.FirstOrDefault(p => p.fId == pIn.fId);
            if(pDb != null)
            {
                pDb.fName = pIn.fName;
                pDb.fQty = pIn.fQty;
                pDb.fCost = pIn.fCost;
                pDb.fPrice = pIn.fPrice;
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
            public ActionResult Delete(int? id)
        {
            if(id != null)
            {
                //先去工廠找查詢的方法(FirstOrDefault),找到點選的
                tProduct prod =db.tProduct.FirstOrDefault(p => p.fId == id);
                if(prod != null)
                {
                    db.tProduct.Remove(prod);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(tProduct p)
        {
            db.tProduct.Add(p);
            db.SaveChanges();
            return RedirectToAction("List");
        }
        // GET: Product
        public ActionResult List()
        {
            string keyword = Request.Form["txtkeyword"];
            //var型別的值不能等於null,所以去畫面(List)上面的@model找到回傳給畫面的值(datas)的型別
            IEnumerable<tProduct> datas = null;
            if (string.IsNullOrEmpty(keyword))
                datas = from p in db.tProduct
                        select p;
            else
                datas =db.tProduct.Where(p=>p.fName.Contains(keyword));
            return View(datas);
        }
    }
}