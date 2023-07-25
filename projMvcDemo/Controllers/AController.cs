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
        public string testingDelete(int? id)
        {
            if (id != null)
                (new CCustomerFactory()).delete((int)id);
            return "刪除資料成功!";
        }

            public string testingInsert()
        {
            CCustomer x = new CCustomer();
            x.fName = "Emma";
            x.fPhone = "0977449520";
            x.fEmail = "eee@gmail.com";
            x.fAddress = "Taipei";
            x.fPassword = "0147";
            (new CCustomerFactory()).creat(x);
            return "新增資料成功!";
        }
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