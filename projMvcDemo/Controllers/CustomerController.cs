using projMvcDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace projMvcDemo.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult List()
        {
            string keyword = Request.Form["txtKeyword"];
            List<CCustomer> datas = null;
            if (string.IsNullOrEmpty(keyword))
                datas = (new CCustomerFactory()).queryAll();
            else
                datas = (new CCustomerFactory()).queryByKeyword(keyword);
            return View(datas);
        }
        public ActionResult Creat()
        {
            return View();
        }
        public ActionResult Save()
        {
            CCustomer x = new CCustomer();
            x.fName = Request.Form["txtName"];
            x.fPhone = Request.Form["txtPhone"];
            x.fEmail = Request.Form["txtEmail"];
            x.fAddress = Request.Form["txtAddress"];
            x.fPassword = Request.Form["txtPassword"];
            (new CCustomerFactory()).creat(x);
            return RedirectToAction("List");
        }
        public ActionResult Delete(int? id)
        {
            if (id != null)
                (new CCustomerFactory()).delete((int)id);
            return RedirectToAction("List");
        }
       
        public ActionResult Edit(int? id)
        {
           if(id == null)
                return RedirectToAction("List");
            CCustomer x = (new CCustomerFactory()).queryById((int)id);
            return View(x);
        }
        
        [HttpPost]
        public ActionResult Edit(CCustomer x)
        {
            (new CCustomerFactory()).update(x);
            return RedirectToAction("List");
        }
    }
}