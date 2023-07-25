using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace projMvcDemo.Models
{
    public class CCustomerFactory
    {
        public void delete(int fid)
        {
            string sql = "DELETE FROM tCustomer WHERE fId=@K_FID";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("K_FID", fid)); //可以不用(object)轉型
            executeSql(sql, paras);
        }
            public void creat(CCustomer p)
        {
            string sql = "INSERT INTO tCustomer (";
            sql += " fName,";
            sql += " fPhone,";
            sql += " fEmail,";
            sql += " fAddress,";
            sql += " fPassword";
            sql += ")VALUES(";
            sql += "@K_FNAME,";
            sql += "@K_FPHONE,";
            sql += "@K_FEMAIL,";
            sql += "@K_FADDRESS,";
            sql += "@K_FPASSWORD)";
            
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("K_FNAME", p.fName));
            paras.Add(new SqlParameter("K_FPHONE", p.fPhone));
            paras.Add(new SqlParameter("K_FEMAIL", p.fEmail));
            paras.Add(new SqlParameter("K_FADDRESS", p.fAddress));
            paras.Add(new SqlParameter("K_FPASSWORD", p.fPassword));
            executeSql(sql,paras);
        }

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
    }
}