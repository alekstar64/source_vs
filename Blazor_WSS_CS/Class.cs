namespace Blazor_WSS_CS
{
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;

    public class IO_MOD
    {
        public struct val_lst
        {
            public string value;
        }

        public struct lst
        {
            public string name;
            public string value;
        }

        public static void Tags_RND()
        {
            foreach (Tag item in TagsList)
            {
                Tag tg = new Tag();
                item.Value += new Random().Next(1);
            }
        }

        public static void Fill_client_delails(int id, string rawtxt)
        {
            List<lst> ret_list = ParseQueryString(rawtxt);
            foreach (lst i in ret_list)
            {
                switch (i.name.ToLower())
                {
                    case "form_name":
                        Clients_LIST[id].form_name = i.value;
                        break;
                    case "tags":
                        Clients_LIST[id].TagsLst = i.value;
                        if (Clients_LIST[id].TagsLst.Length > 0)
                        {
                            List<val_lst> list = ParseArrString(i.value, ',');
                            string _Val = "";
                            foreach (val_lst num in list)
                            {
                                TagsList[Convert.ToInt32(num.value)].subs_lsl_ADD(id, Clients_LIST[id].tags_lst_ADD(Convert.ToInt32(num.value)));
                                Clients_LIST[id].tags_lst_WRITE(Clients_LIST[id].tags_lst.Count - 1, "\"" + TagsList[Convert.ToInt32(num.value)].ID_Name + "\":\"" + TagsList[Convert.ToInt32(num.value)].Value.Replace(",", ".") + "," + TagsList.get_alarm(TagsList[Convert.ToInt32(num.value)].ID) + "\"");
                            }
                        }
                        break;
                    case "ip":
                        Clients_LIST[id].IP = i.value;
                        break;
                    case "machinemame":
                        Clients_LIST[id].MachineName = i.value;
                        break;
                    case "account":
                        Clients_LIST[id].account = i.value;
                        break;
                    case "timeout":
                        if (Convert.ToInt32(i.value) == 0) i.value = "1000";
                        Clients_LIST[id].timeout = Convert.ToInt32(i.value);
                        break;
                    case "attributes":
                        Clients_LIST[id].attributes = Convert.ToInt32(i.value);
                        break;
                }
            }
        }

        public static void Start_stream(short id)
        {
            switch (Clients_LIST[id].attributes)
            {
                case "brief":
                    // Code for "brief" attribute
                    break;
                default:
                    // Code for other cases
                    break;
            }
        }

        public static List<lst> ParseQueryString(string query)
        {
            List<lst> result = new List<lst>();
            if (!string.IsNullOrEmpty(query))
            {
                string[] pairs = query.Substring(1).Split('&', '?');
                foreach (string pair in pairs)
                {
                    string[] parts = pair.Split('=');
                    string name = System.Uri.UnescapeDataString(parts[0]);
                    string value = (parts.Length == 1) ? string.Empty : System.Uri.UnescapeDataString(parts[1]);
                    lst rec = new lst { name = name, value = value };
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
