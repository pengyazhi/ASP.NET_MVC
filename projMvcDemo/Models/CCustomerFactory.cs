using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI.WebControls.WebParts;

namespace projMvcDemo.Models
{
    public class CCustomerFactory
    {
        #region 查詢all
        public List<CCustomer> queryAll()
        {
            string sql = "SELECT *  FROM tCustomer";
            return queryBySql(sql,null);
        }
        #endregion
        
        #region 查詢byID
        public CCustomer queryById(int fId)
        {
            string sql = "SELECT *  FROM tCustomer WHERE fId = @K_FID";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("K_FID", fId));
            List<CCustomer> list = queryBySql(sql, paras);
            if(list.Count ==0)
                return null; 
            return list[0];
        }
        #endregion 

        #region 新增
        public void creat(CCustomer p)
        {
            List<SqlParameter> paras = new List<SqlParameter>();
            string sql = "INSERT INTO tCustomer (";
            //動態判斷使用者是否有填入資料
            if (!string.IsNullOrEmpty(p.fName))
                sql += " fName,";
            if (!string.IsNullOrEmpty(p.fPhone))
                sql += " fPhone,";
            if (!string.IsNullOrEmpty(p.fEmail))
                sql += " fEmail,";
            if (!string.IsNullOrEmpty(p.fAddress))
                sql += " fAddress,";
            if (!string.IsNullOrEmpty(p.fPassword))
                sql += " fPassword,";
            //判斷如果sql指令最後一個字是逗號(,),要去除逗號
            if (sql.Trim().Substring(sql.Trim().Length - 1, 1) == ",") //先將字串去空白後,起始index(sql.Trim().Length-1)等於字串的最後一個index,後面的1是取後面第1個字元
                sql = sql.Trim().Substring(0, sql.Trim().Length - 1);//如果等於逗號(,)則從字串的第0個index取後面整個字串減1的字元數

            sql += ")VALUES(";

            if (!string.IsNullOrEmpty(p.fName))
            {
                sql += "@K_FNAME,";
                paras.Add(new SqlParameter("K_FNAME", p.fName));
            }
            if (!string.IsNullOrEmpty(p.fPhone))
            {
                sql += "@K_FPHONE,";
                paras.Add(new SqlParameter("K_FPHONE", p.fPhone));
            }
            if (!string.IsNullOrEmpty(p.fEmail))
            {
                sql += "@K_FEMAIL,";
                paras.Add(new SqlParameter("K_FEMAIL", p.fEmail));
            }
            if (!string.IsNullOrEmpty(p.fAddress))
            {
                sql += "@K_FADDRESS,";
                paras.Add(new SqlParameter("K_FADDRESS", p.fAddress));
            }
            if (!string.IsNullOrEmpty(p.fPassword))
            {
                sql += "@K_FPASSWORD,";
                paras.Add(new SqlParameter("K_FPASSWORD", p.fPassword));
            }
            if (sql.Trim().Substring(sql.Trim().Length - 1, 1) == ",")
                sql = sql.Trim().Substring(0, sql.Trim().Length - 1);

            sql += ")";

            executeSql(sql, paras);
        }
        #endregion 新增

        #region 修改
        public void update(CCustomer p)
        {
            List<SqlParameter> paras = new List<SqlParameter>();
            string sql = "UPDATE tCustomer SET ";
            if (!string.IsNullOrEmpty(p.fName))
            {
                sql += "fName = @K_FNAME,";
                paras.Add(new SqlParameter("K_FNAME", p.fName));
            }
            if (!string.IsNullOrEmpty(p.fPhone))
            {
                sql += " fPhone =@K_FPHONE,";
                paras.Add(new SqlParameter("K_FPHONE", p.fPhone));
            }
            if (!string.IsNullOrEmpty(p.fEmail))
            {
                sql += "fEmail =@K_FEMAIL,";
                paras.Add(new SqlParameter("K_FEMAIL", p.fEmail));
            }
            if (!string.IsNullOrEmpty(p.fAddress))
            {
                sql += "fAddress =@K_FADDRESS,";
                paras.Add(new SqlParameter("K_FADDRESS", p.fAddress));
            }
            if (!string.IsNullOrEmpty(p.fPassword))
            {
                sql += "fPassword =@K_FPASSWORD,";
                paras.Add(new SqlParameter("K_FPASSWORD", p.fPassword));
            }
          
            if (sql.Trim().Substring(sql.Trim().Length - 1, 1) == ",") 
                sql = sql.Trim().Substring(0, sql.Trim().Length - 1);

            //WHERE前面要空白,不然會導致最後一個宣告的純量變數有錯誤
            sql += " WHERE fId = @K_FID";
            paras.Add(new SqlParameter("K_FID", p.fId));
            executeSql(sql, paras);

        }
        #endregion 修改

        #region 刪除
        public void delete(int fid)
        {
            string sql = "DELETE FROM tCustomer WHERE fId=@K_FID";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("K_FID", fid)); //可以不用(object)轉型
            executeSql(sql, paras);
        }
        #endregion 刪除

        #region SQL查詢共用方法
        public List<CCustomer> queryBySql(string sql, List<SqlParameter> paras)
        {
            List<CCustomer> list = new List<CCustomer>();
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=.;Initial Catalog=dbDemo;Integrated Security=True";
            con.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            if (paras != null)
                cmd.Parameters.AddRange(paras.ToArray());
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                CCustomer x = new CCustomer();
                x.fId = (int)reader["fId"];
                x.fName = reader["fName"].ToString();
                x.fPhone = reader["fPhone"].ToString();
                x.fEmail = reader["fEmail"].ToString();
                x.fAddress = reader["fAddress"].ToString();
                x.fPassword = reader["fPassword"].ToString();
                list.Add(x);
            }
            con.Close();
            return list;
        }
        #endregion 

        #region SQL執行共用方法
        private static void executeSql(string sql,List<SqlParameter>paras)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=.;Initial Catalog=dbDemo;Integrated Security=True";
            con.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            if(paras != null ) 
                cmd.Parameters.AddRange(paras.ToArray());
            cmd.ExecuteNonQuery();
            con.Close();
        }
        #endregion
        public List<CCustomer> queryByKeyword(string keyword)
        {
            string sql = "SELECT *  FROM tCustomer WHERE fName LIKE @K_FKEYWORD";
            sql += " OR fPhone LIKE @K_FKEYWORD";
            sql += " OR fEmail LIKE @K_FKEYWORD";
            sql += " OR fAddress LIKE @K_FKEYWORD";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("K_FKEYWORD", $"%{keyword}%"));
            return queryBySql(sql, paras);
        }
        
    }
}