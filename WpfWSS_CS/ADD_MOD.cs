using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfWSS_CS
{
    using Microsoft.VisualBasic;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class add_mod
    {
        public static Image_lib IL = new Image_lib();
        public static int Delay = 1000;
        public static string JsonText = "{\"items\":[{\"id\": \"tag1\",\"sname\": \"job_control\",\"cur_val\": \"300\"}" +
                                        ",{\"id\": \"tag11\",\"sname\": \"mycontrol\",\"cur_val\": \"310\"}]," +
                                        "\"tags\":[1,2,3,4], " +
                                        "\"now\": \"27.03.2023 21:22:23.44\",\"limit\": 25}";
        public static string MSG_Str = "";
        public static bool Now_Pass = false;
        public static short Last_Min = short.Parse(DateTime.Now.ToString("mm"));
        public static int Last_Count;
        public static MemoryStream img_ms = new MemoryStream();
        public static DateTime init_time;

        [System.Runtime.InteropServices.DllImport("kernel32", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        [System.Runtime.InteropServices.DllImport("kernel32", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

        public static bool Sending = false;
        public static DateTime Last_Send;
        public static int Timer_Send;
        public static bool Auto_Start = false;
        public static string title_APP = "";

        public static int WR_DB;
        public static bool Connect_Err;
        public const int WS_CHILD = 0x40000000;
        public static DateTime Last_Redraw = DateTime.Now;
        public static DateTime Last_Recieved;
        public static bool App_End = false;
        public static bool Thread_Run = false;
        public static string f_io_ini_param(string lpAppName, string lpKeyName, string lpDefault, string lpFileName, bool p_wr)
        {
            int res;
            StringBuilder sb = new StringBuilder(1000);

            if (!p_wr)
            {
                res = GetPrivateProfileString(lpAppName, lpKeyName, lpDefault, sb, sb.Capacity, lpFileName);
                return sb.ToString();
            }
            res = WritePrivateProfileString(lpAppName, lpKeyName, lpDefault, lpFileName);
            return lpDefault;
        }

        public static void save_ini()
        {
           // string ret = "";
            string Local_Path = Directory.GetCurrentDirectory() + "\\config.ini";

            WR_DB = int.Parse(f_io_ini_param("config", "WR_DB", WR_DB.ToString(), Local_Path, true));

            IL.Server_Port = int.Parse(f_io_ini_param("config", "Server_Port", IL.Server_Port.ToString(), Local_Path, true));
            IL.From_Left = int.Parse(f_io_ini_param("config", "From_Left", IL.From_Left.ToString(), Local_Path, true));
            IL.From_TOP = int.Parse(f_io_ini_param("config", "From_TOP", IL.From_TOP.ToString(), Local_Path, true));
            IL.Width = int.Parse(f_io_ini_param("config", "Width", IL.Width.ToString(), Local_Path, true));
            IL.Height = int.Parse(f_io_ini_param("config", "Height", IL.Height.ToString(), Local_Path, true));
            IL.Quality = double.Parse(f_io_ini_param("config", "Quality", IL.Quality.ToString(), Local_Path, true));
            IL.Inverval = int.Parse(f_io_ini_param("config", "Inverval", IL.Inverval.ToString(), Local_Path, true));
            Delay = int.Parse(f_io_ini_param("config", "Delay", Delay.ToString(), Local_Path, true));
            IL.Auto_Start = bool.Parse(f_io_ini_param("config", "Auto_Start", IL.Auto_Start.ToString(), Local_Path, true));

            IL.Proxy_Inverval = int.Parse(f_io_ini_param("config", "Proxy_Inverval", IL.Proxy_Inverval.ToString(), Local_Path, true));
            IL.Proxy_Port = int.Parse(f_io_ini_param("config", "Proxy_Port", IL.Proxy_Port.ToString(), Local_Path, true));
            IL.Proxy_Auto_Start = bool.Parse(f_io_ini_param("config", "Proxy_Auto_Start", IL.Proxy_Auto_Start.ToString(), Local_Path, true));
            IL.Proxy_IP = f_io_ini_param("config", "Proxy_IP", IL.Proxy_IP, Local_Path, true);
            IL.Proxy_Ini_String = f_io_ini_param("config", "Proxy_Ini_String", IL.Proxy_Ini_String, Local_Path, true);
        }

        public static void app_ini()
        {
            string Local_Path = Directory.GetCurrentDirectory() + "\\config.ini";

            IL.Server_Port = int.Parse(f_io_ini_param("config", "Server_Port", "8008", Local_Path, false));
            Timer_Send = int.Parse(f_io_ini_param("config", "Timer_Send", "200", Local_Path, false));
            Auto_Start = bool.Parse(f_io_ini_param("config", "Auto_Start", "False", Local_Path, false));
            title_APP = f_io_ini_param("config", "title_APP", "localHost", Local_Path, false);

            WR_DB = int.Parse(f_io_ini_param("config", "WR_DB", "0", Local_Path, false));
            IL.From_Left = int.Parse(f_io_ini_param("config", "From_Left", "0", Local_Path, false));
            IL.From_TOP = int.Parse(f_io_ini_param("config", "From_TOP", "0", Local_Path, false));
            IL.Width = int.Parse(f_io_ini_param("config", "Width", "100", Local_Path, false));
            IL.Height = int.Parse(f_io_ini_param("config", "Height", "100", Local_Path, false));
            IL.Quality = double.Parse(f_io_ini_param("config", "Quality", "50", Local_Path, false));
            IL.Inverval = int.Parse(f_io_ini_param("config", "Inverval", "2000", Local_Path, false));
            Delay = int.Parse(f_io_ini_param("config", "Delay", "1000", Local_Path, false));
            IL.Auto_Start = bool.Parse(f_io_ini_param("config", "Auto_Start", "False", Local_Path, false));

            IL.Proxy_Inverval = int.Parse(f_io_ini_param("config", "Proxy_Inverval", "2000", Local_Path, false));
            IL.Proxy_Port = int.Parse(f_io_ini_param("config", "Proxy_Port", "8888", Local_Path, false));
            IL.Proxy_Auto_Start = bool.Parse(f_io_ini_param("config", "Proxy_Auto_Start", "False", Local_Path, false));
            IL.Proxy_Ini_String = f_io_ini_param("config", "Proxy_Ini_String", "", Local_Path, false);
            IL.Proxy_IP = f_io_ini_param("config", "Proxy_IP", "", Local_Path, false);
        }

        public static void p_add_log(string p_mess)
        {
            short f_num;
            try
            {
                f_num = (short)FileSystem.FreeFile();
                FileSystem.FileOpen(f_num, DateTime.Now.ToString("dd-MM-yyyy") + ".log", OpenMode.Append);
                FileSystem.PrintLine(f_num, DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " " + p_mess);
                FileSystem.FileClose(f_num);
            }
            catch (Exception )
            {
                // Handle exception
            }
        }


        public static void p_errlog(string p_point, string p_event, string p_msg)
        {
            ushort f_num = (ushort)FileSystem.FreeFile(); 
            try
            {
                //f_num = (ushort)FileSystem.FreeFile();
                FileSystem.FileOpen(f_num, "event_log.txt", OpenMode.Append);
                FileSystem.WriteLine(f_num, DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " | " + p_point + " | " + p_event + " | " + p_msg);
                FileSystem.FileClose(f_num);
            }
            catch (Exception )
            {
                FileSystem.FileClose(f_num);
            }

            FileSystem.FileClose(f_num);
        }
    }
}