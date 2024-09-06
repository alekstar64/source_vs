using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace WpfWSS_CS
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Threading;
    using System.Windows.Media.Imaging;
    
    public  class Image_lib
    {
        public string base64 { get; set; } = "";
        public DateTime image_Time { get; set; } = DateTime.Now;
        public double scaleX { get; set; }
        public double scaleY { get; set; }
        public int Server_Port { get; set; }
        public int From_Left { get; set; }
        public int From_TOP { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Auto_Start { get; set; }
        public string Proxy_Ini_String { get; set; } = "";
        public int Proxy_Port { get; set; }
        public bool Proxy_Auto_Start { get; set; }
        public int Proxy_Inverval { get; set; }
        public string Proxy_IP { get; set; } = "";
        public bool Running { get; set; }
        public int Inverval { get; set; }
        public double Quality { get; set; }
        public BitmapImage imageSource { get; set; } = new BitmapImage();
        public MemoryStream ms { get; set; } = new MemoryStream();
        public byte[] BA { get; set; }

        public FileStream FS { get; set; } = new FileStream("test.jpg", FileMode.Open, FileAccess.Read);
        public Thread th { get; set; }
        //th = new Thread(get_IMG);

        public void Start_TH_ScreenShort()
        {
            th = new Thread(get_IMG);
            if (!th.IsAlive)
            {
                //th.Abort();
                th = new Thread(get_IMG);
                th.Start();
            }
        }

        public void Start_Stop()
        {
            if (Running)
            {
                Running = false;
            }
            else
            {
                Running = true;
                Start_TH_ScreenShort();
            }
        }

        public void load_img()
        {
            FS.Position = 0;
            FS.CopyTo(ms);
            BA = ms.ToArray();
        }

        public void get_IMG()
        {
            do
            {
                ms = new MemoryStream();
                Bitmap Img = new Bitmap(Width, Height);
                Graphics g = Graphics.FromImage(Img);
                g.CopyFromScreen(From_Left, From_TOP, 0, 0, Img.Size);
                g.Dispose();
                Img.Save(ms, ImageFormat.Jpeg);
                Img.Dispose();
                if (Quality != 1)
                {
                    Bitmap newImg = new Bitmap(ms);
                    Bitmap newImg1 = new Bitmap(newImg, Convert.ToInt32(newImg.Width * Quality), Convert.ToInt32(newImg.Height * Quality));
                    ms.SetLength(0);
                    newImg1.Save(ms, ImageFormat.Jpeg);
                    newImg1.Save("ret1.jpg", ImageFormat.Jpeg);
                    newImg1.Dispose();
                    newImg.Dispose();
                }
                image_Time = DateTime.Now;
                base64 = Convert.ToBase64String(ms.ToArray());
                MainWindow.Clients_LIST.Send_Image(base64);
                if (!Running || add_mod.App_End)
                {
                    //th.Abort();
                    break;
                }
                Thread.Sleep(Inverval);
            } while (true);
        }
    }

}
