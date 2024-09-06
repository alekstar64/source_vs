namespace Blazor_WSS_CS
{
    using System;
    using System.Collections;
    using System.Text;
    using System.Text.RegularExpressions;

    public class Tags : CollectionBase
    {
        public int RefreshMS { get; set; } = 1000;

        public string get_alarm(short id)
        {
            if (this[id].MIN_ALARM != 0 && this[id].Value < this[id].MIN_VAL_ALARM)
            {
                return "min";
            }
            else if (this[id].Max_ALARM != 0 && this[id].Value > this[id].Max_VAL_ALARM)
            {
                return "max";
            }
            return "";
        }

        public string ItemValue(object index)
        {
            return "";
        }

        public Tags()
        {
            this.Remove_ALL();
        }

        public void Remove_ALL()
        {
            while (List.Count != 0)
            {
                ((Tag)List[0]).Dispose();
                ((Tag)List[0]).Socket.Close();
                List.RemoveAt(0);
            }
        }

        public int IndexOf(Client value)
        {
            return List.IndexOf(value);
        }

        public void Add(Tag value)
        {
            List.Add(value);
        }

        public bool Contains(Tag value)
        {
            return List.Contains(value);
        }

        public int IndexOf(Tag value)
        {
            return List.IndexOf(value);
        }

        public void Insert(int index, Tag value)
        {
            List.Insert(index, value);
        }

        public int Count
        {
            get { return List.Count; }
        }

        public void Remove(int value)
        {
            List.RemoveAt(value);
        }

        public Tag this[int index]
        {
            get
            {
                try
                {
                    return (Tag)List[index];
                }
                catch (Exception)
                {
                    return null;
                    // Handle exception or log the error
                }
            }
        }
    }

}
