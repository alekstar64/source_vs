using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfWSS_CS
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net.Sockets;
    using WpfWSS_CS;
    using static WpfWSS_CS.Client;

    public class IO_MOD
    {
        public struct val_lst
        {
            public string value;
        }

        public struct Lst
        {
            public string name;
            public string value;
        }

        public static void Tags_RND()
        {
            foreach (Tag item in Dispatch.TagsList)
            {
                Tag tg = new Tag();
                item.Value += new Random().Next(1);
            }
        }

        public static void Fill_client_delails(int id, string rawtxt)
        {
            List<Lst> ret_list = ParseQueryString(rawtxt);
            foreach (Lst i in ret_list)
            {
                switch (i.name.ToLower())
                {
                    case "form_name":
                        MainWindow.Clients_LIST[id].form_name = i.value;
                        break;
                    case "tags":
                        MainWindow.Clients_LIST[id].TagsLst = i.value;
                        if (MainWindow.Clients_LIST[id].TagsLst.Length > 0)
                        {
                            List<val_lst> list = ParseArrString(i.value, ',');
                            string _Val = "";
                            foreach (val_lst num in list)
                            {
                                //Clients_LIST(id).tags_lst_WRITE(Clients_LIST(id).tags_lst.Count - 1, """" & TagsList(Val(num.value)).ID_Name & """:""" & Replace(TagsList(Val(num.value)).Value, ",", ".") &
                                  //  "," & TagsList.get_alarm(TagsList(Val(num.value)).ID) & """")
                                Dispatch.TagsList[Convert.ToInt32(num.value)].subs_lsl_ADD(
                                    (short) id, 
                                    MainWindow.Clients_LIST[id].tags_lst_ADD(short.Parse(num.value)));
                                // Рассчитываем и заполняем значение последнего тега
                                _Val = ("" + Dispatch.TagsList[Convert.ToInt32(num.value)].Value).Replace(",", ".");
                                //MainWindow.Clients_LIST[id].tags_lst_WRITE(
                                //    (short) (MainWindow.Clients_LIST[id].tags_lst.Count - 1),
                                //    "\"" + Dispatch.TagsList[Convert.ToInt32(num.value)].ID_Name + "\":\"" +
                                //    _Val +
                                //    "\"," + Dispatch.TagsList.get_alarm((short) Dispatch.TagsList[short.Parse(num.value)].ID) + "\"");
                                MainWindow.Clients_LIST[id].tags_lst_WRITE((short) (MainWindow.Clients_LIST[id].tags_lst.Count - 1),
                                        "\"" + Dispatch.TagsList[Convert.ToInt32(num.value)].ID_Name + "\":\"" +
                                        _Val + "," +
                                        Dispatch.TagsList.get_alarm((short) Dispatch.TagsList[Convert.ToInt32(num.value)].ID) + "\"");

                                //MainWindow.Clients_LIST[id].tags_lst_WRITE(MainWindow.Clients_LIST[id].tags_lst.Count - 1,
                                //                                    "\"" + Dispatch.TagsList[Convert.ToInt32(num.value)].ID_Name + "\":\"" +
                                //                                    Dispatch.TagsList[Convert.ToInt32(num.value)].Value.Replace(",", ".") +
                                //                                    "\"," + TagsList.get_alarm(TagsList[Convert.ToInt32(num.value)].ID) + "\"");

                            }
                        }
                        break;
                    case "ip":
                        MainWindow.Clients_LIST[id].IP = i.value;
                        break;
                    case "machinemame":
                        MainWindow.Clients_LIST[id].MachineName = i.value;
                        break;
                    case "account":
                        MainWindow.Clients_LIST[id].account = i.value;
                        break;
                    case "timeout":
                        if (Convert.ToInt32(i.value) == 0) MainWindow.Clients_LIST[id].timeout = 1000;
                        MainWindow.Clients_LIST[id].timeout = Convert.ToInt32(i.value);
                        break;
                    case "attributes":
                        MainWindow.Clients_LIST[id].attributes = i.value;
                        break;
                }
            }
        }

        public static void Start_stream(short id)
        {
            switch (MainWindow.Clients_LIST[id].attributes)
            {
                case "brief":
                    // Code for "brief" attribute
                    break;
                default:
                    // Code for other cases
                    break;
            }
        }

        public static List<Lst> ParseQueryString(string query)
        {
            List<Lst> result = new List<Lst>();
            if (!string.IsNullOrEmpty(query))
            {
                string[] pairs = query.Substring(1).Split('&', '?');
                foreach (string pair in pairs)
                {
                    string[] parts = pair.Split('=');
                    string name = System.Uri.UnescapeDataString(parts[0]);
                    string value = (parts.Length == 1) ? string.Empty : System.Uri.UnescapeDataString(parts[1]);
                    Lst rec = new Lst { name = name, value = value };
                    result.Add(rec);
                }
            }
            return result;
        }

        public static List<val_lst> ParseArrString(string query, char delim)
        {
            List<val_lst> result = new List<val_lst>();
            if (!string.IsNullOrEmpty(query))
            {
                string[] pairs = query.Split(delim);
                foreach (string pair in pairs)
                {
                    val_lst rec = new val_lst { value = pair };
                    result.Add(rec);
                }
            }
            return result;
        }
    }

}
