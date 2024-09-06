using System;
using System.Collections.Generic;

namespace WpfWSS_CS
{
    //using WSS.Client;

    public class Tag
    {
        public int ID { get; set; }
        public double MIN_VAL_ALARM { get; set; }
        public double MAX_VAL_ALARM { get; set; }
        public short MIN_ALARM { get; set; } // 1 = OK
        public short MAX_ALARM { get; set; } // 1 = OK
        public int DB_ID { get; set; } //DB ID
        public string FName { get; set; } //Full name
        public string СName { get; set; } //Short name
        public string field_Name { get; set; } //site field or web name
        public string ID_Name { get; set; } //site field id name
        public string Web_Obj_Name { get; set; } //Object [SVG] name or empty
        public short Scale { get; set; } = 99; //round scale
        public short Action { get; set; } //Action animation
        public List<subs_lsl> Subs { get; set; } //Subscribers list

        public struct subs_lsl
        {
            public short subs_index; //reference to websick_client
            public short list_index; //reference to  list of tags in websick_client
        }

        public void subs_lsl_ADD(short subs_index, short list_index)
        {
            subs_lsl _sub = new subs_lsl
            {
                subs_index = subs_index,
                list_index = list_index
            };
            this.Subs.Add(_sub);
        }

        private double c_Value;

        public Tag()
        {
            this.Subs = new List<subs_lsl>();
        }

        public short SubsDEL
        {
            set
            {
                // delete referance from Tag to Subsciber
                for (short i = 0; i < this.Subs.Count; i++)
                {
                    if (this.Subs[i].subs_index == value)
                    {
                        this.Subs.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private string get_alarm(short id)
        {
            if (this.MIN_ALARM != 0 && this.Value < this.MIN_VAL_ALARM)
            {
                return "min";
            }
            else if (this.MAX_ALARM != 0 && this.Value > this.MAX_VAL_ALARM)
            {
                return "max";
            }
            return "";
        }

        public double Value
        {
            set
            {
                this.c_Value = Math.Round(value, this.Scale);

                if (this.Subs.Count > 0)
                {
                    string _Val = $"\"{this.ID_Name}\":\"{this.c_Value.ToString().Replace(",", ".")},{this.get_alarm((short)this.ID)}\"";
                    foreach (subs_lsl _subs in this.Subs)
                    {
                        try
                        {
                            MainWindow.Clients_LIST[_subs.subs_index].tags_lst_WRITE(_subs.list_index, _Val);
                        }
                        catch (Exception)
                        {
                            // Handle exception
                        }
                    }
                }
            }

            get
            {
                return this.c_Value;
            }
        }
    }


}
