using projMvcDemo.Models;
using projMvcDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace projMvcDemo.Controllers
{
    public class CommonController : Controller
    {
        // GET: Common
        public ActionResult Home()
        {
            if (Session[CDictionary.SK_LOGIN_USER] == null)
                return RedirectToAction("Login");
            return View();
        }
        [HttpPost]
        public ActionResult Login(CLoginViewModel vm)
        {
            CCustomer customer = (new CCustomerFactory()).queryByEmail(vm.txtAccount);
            if (customer != null && customer.fPassword.Equals(vm.txtPassword))
            {
                Session[CDictionary.SK_LOGIN_USER] = customer;
                return RedirectToAction("Home");
            }

            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
    }
}