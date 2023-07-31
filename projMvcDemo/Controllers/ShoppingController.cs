using projMvcDemo.Models;
using projMvcDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace projMvcDemo.Controllers
{
    public class ShoppingController : Controller
    {
        // GET: Shopping
        dbDemoEntities db = new dbDemoEntities();
        public ActionResult List()
        {
            var datas = from p in db.tProduct select p;
            return View(datas);
        }
        public ActionResult AddToCart(int? id)
        {
            //先用id找要加入購物車的產品
            if (id == null)
                return RedirectToAction("List");
            ViewBag.FID = id;
            return View();
        }
        [HttpPost]
        public ActionResult AddToCart(CAddToCartViewModel vm)
        {
            //先用id找要加入購物車的產品
            tProduct prod = db.tProduct.FirstOrDefault(p => p.fId == vm.txtFId);
            if (prod == null)
                return RedirectToAction("List");
            tShoppingCart x = new tShoppingCart();
            x.fCount = vm.txtCount;
            x.fProductId = vm.txtFId;
            x.fCustomerId = 1;
            x.fDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            x.fPrice = prod.fPrice;
            db.tShoppingCart.Add(x);
            db.SaveChanges();
            return RedirectToAction("List");
        }

        public ActionResult AddToSession(int? id)
        {
            if (id == null)
                return RedirectToAction("List");
            ViewBag.FID = id;
            return View();
        }
        [HttpPost]
        public ActionResult AddToSession(CAddToCartViewModel vm)
        {
            tProduct prod = db.tProduct.FirstOrDefault(p => p.fId == vm.txtFId);
            if (prod == null)
                return RedirectToAction("List");
            //先找出購物車
            List<CShoppingCartItem> cart = Session[CDictionary.SK_PURCHASED_PRODUCTS_LIST] as List<CShoppingCartItem>;
            //如果沒有購物車,做一台購物車
            if (cart == null)
            {
                cart = new List<CShoppingCartItem>();
                Session[CDictionary.SK_PURCHASED_PRODUCTS_LIST] = cart;
            }

            CShoppingCartItem item = new CShoppingCartItem();
            item.price = (decimal)prod.fPrice;
            item.count = vm.txtCount;
            item.productId = vm.txtFId;
            item.product = prod;
            cart.Add(item);
            return RedirectToAction("List");
        }
        public ActionResult CartView()
        {
            List<CShoppingCartItem> cart = Session[CDictionary.SK_PURCHASED_PRODUCTS_LIST] as List<CShoppingCartItem>;
            if(cart==null)
                return RedirectToAction("List");
            return View(cart);
        }
    }
}