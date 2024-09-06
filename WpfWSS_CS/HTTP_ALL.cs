using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfWSS_CS
{
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;
    using System.Net;
    //using WSS.Client;
    //using Ma.netservice;
    using Microsoft.VisualBasic;

    public static class HttpAll
    {
        public static ulong v_http_conflict_count;
        public static ulong v_protectoin;
        public static bool chk_Deny_Enable = false;
        public static List<string> Deny_List = new List<string>();
        public static List<string> Users_List = new List<string>();
        public static double sim_1;
        public static double sim_2;
        public static double sim_3;
        public static bool send_data = false;
        public static bool simul_data = false;
        public static bool Wait_Bool = false;
        //public static MainWindow.Clients Clients_LIST = new Clients();
        public static long count_PAC, count_Byte, count_Byte_S, count_PAC_S, count_Err;
        public static long count_PAC_REC, count_Byte_REC, count_Byte_S_REC, count_PAC_S_REC, count_Err_REC;
        public static int Size_For_Send;
        public static short Wait_MSeconds;
        public static string Global_Data_For_Send;

        public static int F_ADD_CLIENT(string p_url, Socket p_Socket)
        {
            string[] HostArr = null;
            string HostName = "";

            if (!F_Get_Allow(p_url))
            {
                p_Socket.Close();
                return -1;
            }

            try
            {
                HostArr = p_url.Split(':');
                IPHostEntry myHost = Dns.GetHostEntry(HostArr[0]);
                HostName = myHost.HostName;

                if (!F_Get_Allow(HostName))
                {
                    p_Socket.Close();
                    return -1;
                }

                Users_List.Add(HostName);
            }
            catch (Exception )
            {
                HostName = "no DNSname";
            }

            for (int i = 0; i < MainWindow.Clients_LIST.Count; i++)
            {
                if (MainWindow.Clients_LIST[i].URL == p_url)
                {
                    return -1;
                }
                else if (!MainWindow.Clients_LIST[i].IsActive)
                {
                    MainWindow.Clients_LIST[i].URL = p_url;
                    MainWindow.Clients_LIST[i].ID = i;
                    MainWindow.Clients_LIST[i].Connected = DateTime.Now;
                    MainWindow.Clients_LIST[i].Stat = 0;
                    MainWindow.Clients_LIST[i].Socket = p_Socket;
                    MainWindow.Clients_LIST[i].IsActive = true;
                    MainWindow.Clients_LIST[i].MachineName = HostName;
                    MainWindow.Clients_LIST[i].ProcessReceive();
                    return i;
                }
            }

            MainWindow.Clients_LIST.Add(new Client(p_url, p_Socket));
            MainWindow.Clients_LIST[MainWindow.Clients_LIST.Count - 1].ID = MainWindow.Clients_LIST.Count - 1;
            MainWindow.Clients_LIST[MainWindow.Clients_LIST.Count - 1].MachineName = HostName;
            MainWindow.Clients_LIST[MainWindow.Clients_LIST.Count - 1].ProcessReceive();
                return MainWindow.Clients_LIST.Count - 1;
        }

        public static bool F_Get_Allow(string Str_Deny)
        {
            if (!chk_Deny_Enable)
            {
                return true;
            }

            bool v_ret = true;

            for (int j = 0; j < 2; j++)
            {
                switch (j)
                {
                    case 0:
                        foreach (string item in Deny_List)
                        {
                            if (Str_Deny.ToUpper().Contains(item.Substring(3)) && item.Substring(0, 3) == "DN=")
                            {
                                v_ret = false;
                            }
                        }
                        break;

                    case 1:
                        foreach (string item in Deny_List)
                        {
                            if (Str_Deny.ToUpper().Contains(item.Substring(3)) && item.Substring(0, 3) == "AL=")
                            {
                                v_ret = true;
                            }
                        }
                        break;
                }
            }

            return v_ret;
        }

        public static void SocketException(Socket e, short p_ind)
        {
            try
            {
                MainWindow.Clients_LIST[p_ind].IsActive = false;
                P_Errlog("SockExeption ", MainWindow.Clients_LIST[p_ind].URL.PadRight(23) + "|" + "Delete_Client".PadRight(30) + " | " +
                         MainWindow.Clients_LIST[p_ind].UserName.PadRight(23), "Diconnect");
            }
            catch (Exception)
            {
            }

            try
            {
                e.Close();
            }
            catch (Exception)
            {
                P_Errlog("SocketException", "Close " + MainWindow.Clients_LIST[p_ind].Connected + " " + MainWindow.Clients_LIST[p_ind].URL, "Diconnect");
            }

            try
            {
                string[] list = ParseArrString(MainWindow.Clients_LIST[p_ind].TagsLst, ',');
                foreach (var num in list)
                {
                    Dispatch.TagsList[Convert.ToInt32(num)].SubsDEL = p_ind;
                }

                while (MainWindow.Clients_LIST[p_ind].tags_lst.Count > 0)
                {
                    MainWindow.Clients_LIST[p_ind].tags_lst.RemoveAt(0);
                }

                MainWindow.Clients_LIST[p_ind].TEXTBOX_BUFF = "";
                MainWindow.Clients_LIST[p_ind].UserName = "";

                for (int i = MainWindow.Clients_LIST.Count - 1; i >= 0; i--)
                {
                    if (!MainWindow.Clients_LIST[i].IsActive)
                    {
                        MainWindow.Clients_LIST.RemoveAt(i);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception)
            {
                try
                {
                    P_Errlog("SocketException", "Clients_LIST.RemoveAt(p_ind) " + MainWindow.Clients_LIST[p_ind].Connected + " " +
                                        MainWindow.Clients_LIST[p_ind].URL, "Disconnect");
                }
                catch (Exception)
                {
                    P_Errlog("SocketException", "Clients_LIST.RemoveAt(p_ind) " + "Clients_LIST(p_ind).Connected" + " " +
                                        "Clients_LIST(p_ind).URL", "Disconnect");
                }
            }
        }

        public static string[] ParseArrString(string input, char separator)
        {
            return input.Split(separator);
        }

        public static void P_Errlog(string p_point, string p_event, string p_msg)
        {
            try
            {
                int f_num = Convert.ToInt16(FreeFile());
                FileOpen(f_num, DateTime.Now.ToString("dd-MM-yyyy") + ".log", OpenMode.Append);
                WriteLine(f_num, DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " " + p_point + " | " + p_event + " | " + p_msg);
                FileClose(f_num);
            }
            catch (Exception)
            {
            }
        }

        public static int FreeFile()
        {
            // Реализуйте логику получения свободного номера файла.
            return 0;
        }

        public static void FileOpen(int fileNumber, string fileName, OpenMode mode)
        {
            // Реализуйте логику открытия файла с заданным номером.
        }

        public static void FileClose(int fileNumber)
        {
            // Реализуйте логику закрытия файла с заданным номером.
        }

        public static void WriteLine(int fileNumber, string line)
        {
            // Реализуйте логику записи строки в файл с заданным номером.
        }

    }

}
