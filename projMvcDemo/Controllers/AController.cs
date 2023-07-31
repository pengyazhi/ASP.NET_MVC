using projMauiDemo.Resources.Models;
using projMvcDemo.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace projMvcDemo.Controllers
{
    public class AController : Controller
    {
        public ActionResult demoFileUpload()
        {
            return View();
        }
        [HttpPost]
        public ActionResult demoFileUpload(HttpPostedFileBase photo)
        {
            photo.SaveAs(@"C:\QNote\codes\slnMvcDemo\projMvcDemo\Images\001.jpg");
            return View();
        }
        //使用cookie不會因為重啟網頁而被更新,但容易被攻擊跟被使用者清除掉
        public ActionResult showCountByCookie() 
        {
            int count = 0;
            HttpCookie cookie = Request.Cookies["KK"];
            if(cookie != null)
                count = Convert.ToInt32(cookie.Value);
            count++;
            cookie = new HttpCookie("KK");
            cookie.Value = count.ToString();
            cookie.Expires = DateTime.Now.AddSeconds(10);
            Response.Cookies.Add(cookie);

            ViewBag.COUNT = count;
            return View();
        }

            public ActionResult showCountBySession() //如果用的是分頁開啟另一個相同網址也會變成同步(SessionID是一樣的)
        {
            int count = 0;
            if (Session["COUNT"] != null)
                count = (int)Session["COUNT"];
            count++;
            Session["COUNT"] = count;
            ViewBag.COUNT = count;
            return View();
        }
        static int count = 0; //異地同步
        public ActionResult showCount()
        {
            count++;
            ViewBag.N = count;
            return View();
        }
            public ActionResult demoForm() 
        {
            ViewBag.ANS = "?";
            if (!string.IsNullOrEmpty(Request.Form["txtA"])
                && !string.IsNullOrEmpty(Request.Form["txtB"])
                && !string.IsNullOrEmpty(Request.Form["txtC"]))
            {
                double a = ViewBag.A = Convert.ToDouble(Request.Form["txtA"]);
                double b = ViewBag.B = Convert.ToDouble(Request.Form["txtB"]);
                double c = ViewBag.C = Convert.ToDouble(Request.Form["txtC"]);
                double sqrt = Math.Sqrt((b * b) - 4 * a * c);
                ViewBag.ANS = $"{(-b + sqrt) / 2 * a} 或 {(-b - sqrt) / 2 * a} " ;
            }
            return View();
        }
        #region 查詢全部資料測試
        public string testingQuery()
        {
            return  "目前客戶數 : " + (new CCustomerFactory()).queryAll().Count().ToString();
        }
        #endregion

        #region 查詢單筆資料測試ByID
        public string testingQueryById(int? id)
        {
            if(id != null)
            {
                CCustomerFactory x = new CCustomerFactory();
                CCustomer customer = x.queryById((int)id);
                if (customer != null)
                    return $"客戶名稱 : {customer.fName} / {customer.fPhone} / {customer.fAddress} / {customer.fEmail}";
                return "沒有符合的查詢";
            }
            return "沒有提供查詢條件";
        }
        #endregion

        #region 刪除資料測試
        public string testingDelete(int? id)
        {
            if (id != null)
                (new CCustomerFactory()).delete((int)id);
            return "刪除資料成功!";
        }
        #endregion 刪除資料測試

        #region 更新資料測試
        public string testingUpdate()
        {
            CCustomer x = new CCustomer();
            x.fId = 9;
            x.fName = "Terry";
            x.fPhone = "0977449520";
            x.fEmail = "eee@gmail.com";
            x.fAddress = "PinDung";
            x.fPassword = "7777";
            (new CCustomerFactory()).update(x);
            return "修改資料成功!";
        }
        #endregion 更新資料測試

        #region 新增資料測試
        public string testingInsert()
        {
            CCustomer x = new CCustomer();
            x.fName = "Terry";
            //x.fPhone = "0977449520";
            //x.fEmail = "eee@gmail.com";
            x.fAddress = "TaiDung";
            x.fPassword = "5555";
            (new CCustomerFactory()).creat(x);
            return "新增資料成功!";
        }
        #endregion
        //強型別:將物件放到return View , cshtml使用@model及Model
        public ActionResult bindingById(int? id)
        { 
            CCustomer x = null; 
            if (id != null)
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = @"Data Source=.;Initial Catalog=dbDemo;Integrated Security=True";
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"SELECT * FROM tCustomer WHERE fId = {id}";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                  x = new CCustomer();
                    x.fId = (int)reader["fId"];
                    x.fName = reader["fName"].ToString();
                    x.fPhone = reader["fPhone"].ToString();
                    x.fEmail = reader["fEmail"].ToString();
                }
                con.Close();
            }
            return View(x);
        }
        //弱型別,使用ViewBag/ViewData加上key及value
        public ActionResult showById(int? id)
        {
            //ViewBag.KK = "沒有任何資料符合查詢條件"; <<因為ViewBag.KK現在是CCustomer型別
            if (id != null)
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = @"Data Source=.;Initial Catalog=dbDemo;Integrated Security=True";
                con.Open();
                //string s = "沒有任何資料符合查詢條件"; << 被ViewBag取代
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"SELECT * FROM tCustomer WHERE fId = {id}";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    //如果用一組key對應一筆資料就是一個欄位一筆資料的思考方式 *應該創建類別,讓類別變成一個key裡面放很多資料*
                    //ViewBag.NAME = reader["fName"].ToString() ;
                    //ViewBag.PHONE = reader["fPhone"].ToString();
                    //ViewBag.ID = reader["fId"].ToString() ;
                    //ViewBag.EMAIL = reader["fEmail"].ToString();
                    CCustomer x = new CCustomer();
                    x.fId = (int)reader["fId"];
                    x.fName = reader["fName"].ToString();
                    x.fPhone = reader["fPhone"].ToString();
                    x.fEmail = reader["fEmail"].ToString();
                    ViewBag.KK = x;
                }
                con.Close();
                
            }
            return View();
        }
        public string demoServer()
        {
            return "目前伺服器上的實體位置：" + Server.MapPath(".");
        }
        public string queryById(int? id)
        {
            if (id == null)
                return "沒有指定id";
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=.;Initial Catalog=dbDemo;Integrated Security=True";
            con.Open();

            string s = "沒有任何資料符合查詢條件";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = $"SELECT * FROM tCustomer WHERE fId = {id}";
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                s = reader["fName"].ToString() + "/" + reader["fPhone"].ToString();
            }
            con.Close();

            return s;
        }
        public string demoRequest()
        {
            string id = Request.QueryString["pid"];
            if (id == "0")
                return "蘭嶼三天兩夜行程 加入購物車成功";
            else if (id == "1")
                return "澎湖花火節兩天一夜行程 加入購物車成功";
            else if (id == "2")
                return "花蓮遠雄海洋公園門票 加入購物車成功";
            return "找不到該產品";
        }
        public string demoParm(int? id)
        {
            if (id == 0)
                return "蘭嶼三天兩夜行程 加入購物車成功";
            else if (id == 1)
                return "澎湖花火節兩天一夜行程 加入購物車成功";
            else if (id == 2)
                return "花蓮遠雄海洋公園門票 加入購物車成功";
            return "找不到該產品";
        }
        public string lotto()
        {
            //Models加的是「共用方法」
            //直接加入現有的class並直接new一塊新的記憶體呼叫裡面的方法
            return (new CLottoGen()).getNumbers();
        }
        public string sayHello()
        {
            return "Hello, ASP.NET MVC";
        }
        // GET: A
        public ActionResult Index()
        {
            return View();
        }
    }
}