using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfWSS_CS
{
    public class Dispatch
    {
        public static Tags TagsList = new Tags();
        public static Tag Tag = new Tag();

        public static void RND_Value()
        {
            for (short i = 0; i < TagsList.Count; i++)
            {
                TagsList[i].Value = Convert.ToDouble(TagsList[i].Value) + new Random().NextDouble();
            }
        }

        public static void init_Core(short max_index)
        {
            for (int i = 0; i <= max_index; i++)
            {
                Tag tag = new Tag();
                tag.ID = i;
                tag.ID_Name = "tag_id_" + i;
                tag.field_Name = "tag_name_id_" + i;
                tag.Scale = 3;
                tag.Value = new Random().NextDouble() * i;
                TagsList.Add(tag);
            }
        }
    }

}
