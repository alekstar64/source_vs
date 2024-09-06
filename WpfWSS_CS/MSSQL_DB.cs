using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace WpfWSS_CS
{

    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading;

    public static class MSSQL_DB
    {
        public static bool ONS = true;
        public static Thread th = new Thread(p_Th_MSSQL_Tab);
        public static string DATASOURCE = "Server=ALEXHOME\\SQLEXPRESS;database=alex;Trusted_Connection=True;MultipleActiveResultSets=true";

        public static System.Data.SqlClient.SqlConnection MyConnection = new SqlConnection(DATASOURCE);
        private static SqlCommand? MyCommand;
        private static SqlParameter? myparam;
        private static SqlDataReader? myReader;
        public static string? c_Date;
        public static string query = "";

        public static void Start_TH_MSSQL()
        {
            th.Start();
        }

        public static void p_Th_MSSQL_Tab()
        {
            string str = "";

            short cnt;
            do
            {
                if (MyConnection.State == ConnectionState.Open)
                {
                }
                else
                {
                    try
                    {
                        MyConnection = new SqlConnection(DATASOURCE);
                        MyConnection.Open();
                    }
                    catch (Exception ex)
                    {
                        MyConnection = new SqlConnection(DATASOURCE);
                        MyConnection.Open();
                        add_mod.p_add_log("DB connection error");
                    }
                }

                if (ONS)
                {
                    query = "Select id,value,convert(varchar(25), getdate(), 121) AS _CURRENT_TIMESTAMP,id_name,field_name," +
                            " min_val_alarm,max_val_alarm,min_alarm,max_alarm,scale from fast_data order by id";
                }
                else
                {
                    query = "Select id,value,convert(varchar(25), getdate(), 121) AS _CURRENT_TIMESTAMP from fast_data" +
                            " where CDate > Convert(datetime, '" + c_Date + "', 121)";
                }

                if (add_mod.WR_DB == add_mod.WR_DB)
                {
                    try
                    {
                        MyCommand = new SqlCommand("p_change_fast_data", MyConnection);
                        MyCommand.CommandType = CommandType.StoredProcedure;
                        MyCommand.Parameters.AddWithValue("@count", 100);
                        MyCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                    }
                }

                MyCommand = new SqlCommand(query, MyConnection);
                myReader = MyCommand.ExecuteReader();

                while (myReader.Read())
                {
                    cnt = Convert.ToInt16(myReader.GetInt32(0));

                    if (ONS)
                    {
                        Dispatch.TagsList[cnt].ID_Name = myReader.GetString(3);
                        Dispatch.TagsList[cnt].field_Name = myReader.GetString(4);
                        Dispatch.TagsList[cnt].Scale = short.Parse(myReader["scale"].ToString());
                        Dispatch.TagsList[cnt].MIN_VAL_ALARM = Convert.ToDouble(myReader["min_val_alarm"]);
                        Dispatch.TagsList[cnt].MAX_VAL_ALARM = Convert.ToDouble(myReader["max_val_alarm"]);
                        Dispatch.TagsList[cnt].MIN_ALARM = short.Parse(myReader["MIN_ALARM"].ToString());
                        Dispatch.TagsList[cnt].MAX_ALARM = short.Parse(myReader["max_alarm"].ToString());  
                    }

                    c_Date = myReader.GetString(2);
                    Dispatch.TagsList[cnt].Value = Convert.ToDouble(myReader[1]);
                }

                myReader.Close();
                MyCommand.Dispose();
                MyConnection.Close();
                ONS = false;
                add_mod.Thread_Run = true;
                Thread.Sleep(add_mod.Delay);

                if (add_mod.App_End)
                {
                    add_mod.Thread_Run = false;
                    break;
                }
            }
            while (true);
        }
    }

}
