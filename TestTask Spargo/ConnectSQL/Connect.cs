using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask_QA.ConnectSQL
{
    public class Connect
    {
        static public DataSet GetDatas(string cmd) //Достает с базы PSIGMA FLAT и преобразует в грид, таблицу
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source=(LocalDb)\WS150002\SQLEXPRESS; Initial Catalog= QA; integrated security=True;");
                SqlCommand c = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();
                sqlcon.Open();
                c = sqlcon.CreateCommand();
                c.CommandText = cmd;
                da.SelectCommand = c;
                da.Fill(ds, "Table1");
                sqlcon.Close();
                return ds;
            }

            catch (Exception mes)
            {
                Console.WriteLine(mes.ToString());
                return null;
            }

        }

        public static object SelectString(string cmd) //Достает с базы PSIGMA FLAT строковые значения и числовые
        {

            SqlConnection sqlcon = new SqlConnection(@"Data Source=(LocalDb)\WS150002\SQLEXPRESS; Initial Catalog= QA; integrated security=True;");
            SqlCommand c = new SqlCommand();
            SqlDataReader r;
            string k = "";
            c = sqlcon.CreateCommand();
            c.CommandType = CommandType.Text;
            c.CommandText = cmd;
            try
            {
                sqlcon.Open();
                r = c.ExecuteReader();
                if (r.Read())
                {
                    k = r.GetString(0);
                    r.Close();
                }

                sqlcon.Dispose();
                return k;
            }


            catch (Exception mes)
            {
                Console.WriteLine(mes.ToString());
                return "";
            }

        }

        public static int SelectStringInt(string cmd) //Достает с базы PSIGMA FLAT строковые значения и числовые
        {

            SqlConnection sqlcon = new SqlConnection(@"Data Source=(LocalDb)\WS150002\SQLEXPRESS; Initial Catalog= QA; integrated security=True;");            
            SqlCommand c = new SqlCommand();
            SqlDataReader r;
            int k = 0;
            c = sqlcon.CreateCommand();
            c.CommandType = CommandType.Text;
            c.CommandText = cmd;
            try
            {
                sqlcon.Open();
                r = c.ExecuteReader();
                if (r.Read())
                {
                    k = r.GetInt32(0);
                    r.Close();
                }

                sqlcon.Dispose();
                return k;
            }


            catch (Exception mes)
            {
                Console.WriteLine(mes.ToString());
                return 0;
            }

        }
    }
}
