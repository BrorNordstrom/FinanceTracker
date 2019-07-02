using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NordIT.Classes
{
    public class global
    {
        private SQLiteConnection sql_con;
        private SQLiteCommand sql_cmd;
        private SQLiteDataAdapter DB;
        private DataTable DT = new DataTable();
        private static string username;

        public string UserName { get { return username; } set { username = value; } }

        public void SetConnection()
        {
            try
            {
                sql_con = new SQLiteConnection
               ("Data Source=NordITDB.sqlite;Version=3;New=False;Compress=True;");
               
            }
            catch (Exception ex)
            {

            }

        }
        //Get Payee ID
        public int getID(String name)
        {
            SetConnection();
            sql_con.Open();
            string selectquery = "select id from tbl_Payees where name =?";
            sql_cmd = sql_con.CreateCommand();
            sql_cmd.CommandText = selectquery;
            sql_cmd.Parameters.AddWithValue("@name", name);
            SQLiteDataReader rdr = sql_cmd.ExecuteReader();
            int ID = 0;
            while (rdr.Read())
            {
                ID = rdr.GetInt32(0);
            }
            sql_con.Close();
            return ID;
        }
        //Get Payer ID
        public int getPayerID(String name)
        {
            SetConnection();
            sql_con.Open();
            string selectquery = "select id from tbl_Payers where name =?";
            sql_cmd = sql_con.CreateCommand();
            sql_cmd.CommandText = selectquery;
            sql_cmd.Parameters.AddWithValue("@name", name);
            SQLiteDataReader rdr = sql_cmd.ExecuteReader();
            int ID = 0;
            while (rdr.Read())
            {
                ID = rdr.GetInt32(0);
            }
            sql_con.Close();
            return ID;
        }
        public async Task ExecuteQuery(string txtQuery)
        {
            SetConnection();
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();
            sql_cmd.CommandText = txtQuery;
            await sql_cmd.ExecuteNonQueryAsync();
            sql_con.Close();
        }

        public int ExecuteInsertQuery(string txtQuery)
        {
            
            SetConnection();
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();
            sql_cmd.CommandText = txtQuery;
           var isertedid =  sql_cmd.ExecuteScalar();
            sql_con.Close();

            return int.Parse(isertedid.ToString());
        }

        public Task<DataTable> GetDataTable(string txtQuery)
        {
            DataSet DS = new DataSet();

            return Task.Run(() =>
            {
                SetConnection();
                sql_con.Open();
                sql_cmd = sql_con.CreateCommand();

                DB = new SQLiteDataAdapter(txtQuery, sql_con);
                DS.Reset();
                DB.Fill(DS);
                DT = DS.Tables[0];
                
                sql_con.Close();

                return DT;
            });
        }


    }
}
