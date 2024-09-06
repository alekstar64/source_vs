using Microsoft.VisualBasic;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Net.WebSockets;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.AspNetCore.Hosting.Server;

namespace WpfWSS_CS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public WebSocketServer server = new WebSocketServer();
        //await Task.Run(() => server.StartAsync(12345));
        public long minmem = 0;
        //DispatcherTimer timer = new DispatcherTimer();
        public DispatcherTimer Timer_Stat  = new DispatcherTimer();
        private void Timer_Stat_Tick(object sender, EventArgs e)
        {
            
            //DB.p_get_STAT();
            //Exit Sub
            //  RND_Value()
            //memory = GC.GetTotalMemory(True)
            if (minmem == 0)
            {
                minmem = GC.GetTotalMemory(true);
            }
            if (add_mod.Last_Min != int.Parse(DateTime.Now.ToString("mm")))
            {
                add_mod.Last_Min = short.Parse(DateTime.Now.ToString("mm"));
                minmem = GC.GetTotalMemory(true);
            }
            //lbl_test.Content = "dfdd"; 
            long c_mem = GC.GetTotalMemory(true);
            lblmem.Content = $"{minmem} / {GC.GetTotalMemory(true)} +/- {c_mem - minmem}";
            //SolidColorBrush br = new SolidColorBrush(Color.FromArgb(255, 255, 139, 0));
            if (c_mem - minmem > 0)
            {
                //lblmem.Background = new SolidColorBrush(Color.FromArgb(255, 255, 139, 139));
            }
            else
            {
                //lblmem.Background = new SolidColorBrush(Color.FromArgb(255, 139, 255, 139));
            }
            //lblmem.Content = Marshal.SizeOf(CObj(Clients_LIST));
            //Exit Sub
            lbl_Time.Content = " " + DateTime.Now.ToString();
            lbl_Byte_S.Content = (HttpAll.count_Byte_S / 1000).ToString();
            lbl_PAC_S.Content = HttpAll.count_PAC_S.ToString();
            HttpAll.count_Byte += HttpAll.count_Byte_S;
            HttpAll.count_PAC += HttpAll.count_PAC_S;
            lbl_Byte.Content = (HttpAll.count_Byte / 1000).ToString();
            lbl_Pac.Content = HttpAll.count_PAC.ToString();
            lbl_Err.Content = HttpAll.count_Err.ToString();
            HttpAll.count_Byte_S = 0;
            HttpAll.count_PAC_S = 0;
            lbl_Byte_S_REC.Content = HttpAll.count_Byte_S_REC.ToString();
            lbl_PAC_S_REC.Content = HttpAll.count_PAC_S_REC.ToString();
            HttpAll.count_Byte_REC += HttpAll.count_Byte_S_REC;
            HttpAll.count_PAC_REC += HttpAll.count_PAC_S_REC;
            lbl_Byte_REC.Content = (HttpAll.count_Byte_REC / 1000).ToString();
            lbl_PAC_REC.Content = HttpAll.count_PAC_REC.ToString();
            lbl_ERR_REC.Content = HttpAll.count_Err_REC.ToString();
            HttpAll.count_Byte_S_REC = 0;
            HttpAll.count_PAC_S_REC = 0;
            lbl_Client.Content = "Total clients : " + Clients_LIST.Count + "\n";
            if (Clients_LIST.Count != lst_Clients.Items.Count)
            {
                if (Clients_LIST.Count > lst_Clients.Items.Count)
                {
                    while (Clients_LIST.Count > lst_Clients.Items.Count)
                    {
                        lst_Clients.Items.Add("1");
                    }
                }
                else
                {
                    while (Clients_LIST.Count < lst_Clients.Items.Count)
                    {
                        lst_Clients.Items.RemoveAt(lst_Clients.Items.Count - 1);
                    }
                }
            }
            for (short i = 0; i < Clients_LIST.Count; i++)
            {
                try
                {
                    if ((DateTime.Now - Clients_LIST[i].LastRec).Seconds < 200 && Clients_LIST[i].IsActive)
                    {
                        try
                        {
                            lst_Clients.Items[i] =
                                $"{Clients_LIST[i].ID.ToString().PadLeft(3, ' ')}| " +
                                $"{Clients_LIST[i].Socket.RemoteEndPoint.ToString().PadRight(20, ' ')} | " +
                                $"{(Clients_LIST[i].UserName + " " + Clients_LIST[i].LastMSG).PadRight(25, ' ')} | " +
                                $"{Clients_LIST[i].MachineName.PadRight(30, ' ')} | " +
                                $"{Clients_LIST[i].Connected.ToString("dd.MM.yy HH:mm")} | " +
                                $"{Clients_LIST[i].In_Count}/{Clients_LIST[i].Out_Count}";

                        }
                        catch (Exception ex)
                        {
                            add_mod.p_errlog("SendImage", "SendAsync", ex.Message);
                            HttpAll.SocketException(Clients_LIST[i].Socket, i);
                        }
                        try
                        {
                            Clients_LIST[i].Out_Count = 0;
                            Clients_LIST[i].In_Count = 0;
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else if (!Clients_LIST[i].IsActive)
                    {
                        lst_Clients.Items[i] = $"{Strings.Left((i + 1).ToString() + "    ", 4)}Empty Socket {Strings.Left(Clients_LIST[i].URL + "          ", 20)}";
                    }
                    else
                    {
                        HttpAll.SocketException(Clients_LIST[i].Socket, i);
                        HttpAll.count_Err_REC = HttpAll.count_Err_REC + 1;
                    }
                }
                catch (Exception)
                {

                }
            }
            if (add_mod.IL.Running)
            {
                Btn_Action.Content = "Stop (Started)";
                lbl_screenshrt_state.Content = "SS = True";
            }
            else
            {
                Btn_Action.Content = "Start (Stopped)";
                lbl_screenshrt_state.Content = "SS = False";
            }
        }

        public static Tags TagsList = new Tags();
        public static Tag tag = new Tag();
        //public string JsonText = "";
        public static Clients Clients_LIST = new Clients();
        public MainWindow()
        
        {
            InitializeComponent();
        }

        private void Btn_FIX_Area_Click(object sender, RoutedEventArgs e)
        {
            SetSize();
        }

        private void Btn_Action_Click(object sender, RoutedEventArgs e)
        {
            if (add_mod.IL.Running)
            {
                this.Btn_Action.Content = "Stop (Started)";
                this.lbl_screenshrt_state.Content = "SS = True";
                add_mod.IL.Running = false;
            }
            else
            {
                this.Btn_Action.Content = "Start (Stopped)";
                this.lbl_screenshrt_state.Content = "SS = False";
                add_mod.IL.Running = true;
                add_mod.IL.Start_TH_ScreenShort();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            add_mod.IL.Proxy_Ini_String = this.Proxy_Ini_String.Text;
            add_mod.save_ini();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Dim DBB(0 To 10) As Double
                //DBB(11) = (2 + 1) / 0
                //SendAsync(ResStr);
                //JsonText = Me.Json.Text;
                //Me.JsonText.Text;
                add_mod.IL.get_IMG();
                lblmem.Content = GC.GetTotalMemory(true);
            }
            catch (Exception)
            {
                // Handle exception
            }
        }

        private void sl_Ouality_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                UIElement element = FindName("sl_Ouality_Value") as UIElement;
                if (element != null)
                {
                    lbl_Quality.Content = Math.Round(sl_Ouality.Value, 2);
                    add_mod.IL.Quality = Math.Round(sl_Ouality.Value, 2);
                }
                //Math.Round(1 / (Val(Me.sl_Capacity.Value)) * 1000)
                //Math.Round
            }
            catch (Exception ex)
            {
                add_mod.Delay = 50;
            }
        }


        private void sl_Refresh_Intervel_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                UIElement element = FindName("lbl_Refreh_Interval") as UIElement;
                if (element != null)
                {
                    this.lbl_Refreh_Interval.Content = (Math.Round(this.sl_Refresh_Intervel.Value));
                    add_mod.IL.Inverval = (int)Math.Round(this.sl_Refresh_Intervel.Value);
                }
                //Math.Round(1 / (Val(this.sl_Capacity.Value)) * 1000)
                //Math.Round
            }
            catch (Exception ex)
            {
                add_mod.Delay = 1000;
            }
        }
        private void SetSize()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                add_mod.IL.From_Left = 0;
                add_mod.IL.From_TOP = 0;
            }
            else
            {
                add_mod.IL.From_Left = (int)(this.Left * add_mod.IL.scaleX);
                add_mod.IL.From_TOP = (int)(this.Top * add_mod.IL.scaleY);
            }

            add_mod.IL.Width = (int)(this.ActualWidth * add_mod.IL.scaleX);
            add_mod.IL.Height = (int)(this.ActualHeight * add_mod.IL.scaleY);

            this.lbl_Width.Content = add_mod.IL.Width;
            this.lbl_Height.Content = add_mod.IL.Height;
            this.lbl_UP.Content = add_mod.IL.From_TOP;
            this.lbl_Left.Content = add_mod.IL.From_Left;
        }

        private void Ini_IL()
        {
            this.lbl_Width.Content = add_mod.IL.Width;
            this.lbl_Height.Content = add_mod.IL.Height;
            this.lbl_UP.Content = add_mod.IL.From_TOP;
            this.lbl_Left.Content = add_mod.IL.From_Left;
            this.AutoStart.IsChecked = add_mod.IL.Auto_Start;
            this.sl_Ouality.Value = add_mod.IL.Quality;
            this.sl_Refresh_Intervel.Value = add_mod.IL.Inverval;
            this.sl_Capacity.Value = add_mod.Delay;
            this.lbl_Capacity.Content = add_mod.Delay;
        }
        private async void StartServer()
        {
            WebSocketServer server = new WebSocketServer();
            await Task.Run(() => server.StartAsync(12345)); // Укажите нужный порт
        }
        private void netservice_Loaded(object sender, RoutedEventArgs e)
        {
            StartServer();
            // Dim oledb As New OLEDB
            //OLED'B.sa()
            Img();
            add_mod.app_ini();
            Ini_IL();
            //Me.netservice.Title = title_APP
            HttpAll.Wait_Bool = true; //chkWait.Checked
            HttpAll.simul_data = true; //chk_SIMUL.Checked
            HttpAll.send_data = true; //chk_Send_data.Checked
                              //Me.lbl_Byte = Left("asdf", 1)
            lbl_Start.Content = "Started :  " + DateTime.Now;
            lbl_Time.Content = " " + DateTime.Now;
            if (add_mod.IL.Server_Port == 0)
                add_mod.IL.Server_Port = 8008;
            if (add_mod.Auto_Start)
            {
                //   Me.WindowState = Windows.WindowState.Minimized
            }
            else
            {
                //  Me.WindowState = Windows.WindowState.Normal
            }
            //Exit Sub
            Clients_LIST = new Clients();
            Clients_LIST.NewSocket(add_mod.IL.Server_Port);
            Timer_Stat.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            //Timer_Stat.Interval = new TimeSpan.FromMilliseconds(1000);
            //TimeSpan.FromMilliseconds
            Timer_Stat.Start();
            Timer_Stat.Tick += Timer_Stat_Tick;
            //Dim DB As New DB
            //Exit Sub
            Dispatch.init_Core(1000);
            MSSQL_DB.Start_TH_MSSQL();
            //Delay = 1000
            if (add_mod.IL.Auto_Start)
            {
                // открыть для ИМИДЖА
                add_mod.IL.Running = true;
                add_mod.IL.Start_TH_ScreenShort();

            }
            Proxy_Ini_String.Text = add_mod.IL.Proxy_Ini_String;
            //IL.get_IMG()
            //Start_TH_MSSQL
            //DB.p_get_STAT()
        }

        private void netservice_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            while (Clients_LIST.Count > 0)
            {
                try
                {
                    Clients_LIST[0].Socket.Close();
                    // WPF ClientsShow.Items.RemoveAt(0);
                    Clients_LIST.RemoveAt(0);
                }
                catch (Exception)
                {

                }
            }
        }

        private void netservice_Closed(object sender, EventArgs e)
        {
            Timer_Stat.Stop();
            add_mod.App_End = true;
            while (add_mod.Thread_Run)
            {

            }

            try
            {
                Clients_LIST.Remove_ALL();
                // Clients_LIST.serverSocket.Close();
                Clients_LIST = new Clients();
            }
            catch (Exception)
            {

            }
        }

        private void cmd_Kill_Cl_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = int.Parse(txt_kill_Cl.Text);
                if (Clients_LIST[index].IsActive)
                {
                    add_mod.p_errlog("I killed him ", $"{Strings.Left(Clients_LIST[index].URL, 23)} | {Strings.Left(Clients_LIST[index].MachineName, 30)} | {Strings.Left(Clients_LIST[index].UserName, 23)}", "Killed");
                    HttpAll.SocketException(Clients_LIST[index].Socket,(short) index);
                }
            }
            catch (Exception)
            {
                // Handle exception
            }
        }
        public  void Img()
        {
            add_mod.IL.scaleX = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice.M11;
            add_mod.IL.scaleY = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice.M22;
            add_mod.IL.load_img();
            add_mod.IL.ms.Position = 0;
            add_mod.IL.imageSource.BeginInit();
            add_mod.IL.imageSource.CacheOption = BitmapCacheOption.OnLoad;
            add_mod.IL.imageSource.StreamSource = add_mod.IL.ms;
            add_mod.IL.imageSource.EndInit();
            this.Img_Preview.Source = add_mod.IL.imageSource;
            add_mod.IL.ms.Position = 0;
            byte[] byte0 = new byte[1];
            var ms_byte = byte0;
        }
        private void AutoStart_Click(object sender, RoutedEventArgs e)
        {
            var a = AutoStart.IsChecked;
            add_mod.IL.Auto_Start = (bool)AutoStart.IsChecked;            
        }

        private void sl_Capacity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                UIElement element = FindName("lbl_Capacity") as UIElement;
                if (element != null)
                    this.lbl_Capacity.Content = Math.Round(this.sl_Capacity.Value);
                add_mod.Delay = (int)Math.Round(this.sl_Capacity.Value);
                //Math.Round(1 / (Val(Me.sl_Capacity.Value)) * 1000)
                //Math.Round
            }
            catch (Exception ex)
            {
                add_mod.Delay = 1000;
            }

        }

    }


}
