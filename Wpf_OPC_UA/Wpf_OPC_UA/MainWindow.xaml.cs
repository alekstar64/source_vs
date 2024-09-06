using Opc.Ua;
using Opc.UaFx;
using Opc.UaFx.Client;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Wpf_OPC_UA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        public OpcClient client = new OpcClient(nodeurl);
        public static string nodeurl = "opc.tcp://ALEXHOME:26543";
        public static string[] nodeIds = { "ns=1;i=1001" };
        public static int count;
        public static string vall;
        public static string sec = DateTime.Now.ToString("ss");

        private static void HandleDataChanged(object sender,OpcDataChangeReceivedEventArgs e)
        {
            // The tag property contains the previously set value.
            OpcMonitoredItem item = (OpcMonitoredItem)sender;
            string txt = e.Item.Value.ToString();
            if (sec != DateTime.Now.ToString("ss")) 
                {
                    sec = DateTime.Now.ToString("ss");
                    count = 0;
                }
            count += 1;
            vall = e.Item.Value.ToString();
            //Console.WriteLine(
            //        "Data Change from Index {0}: {1}",
            //        item.Tag,
            //        e.Item.Value);
        }

        public MainWindow()
        {
            InitializeComponent();
            if (client.State != OpcClientState.Connected)
            {
                client.Connect();
            }
            OpcSubscription subscription = client.SubscribeNodes();

            for (int index = 0; index < nodeIds.Length; index++)
            {
                // Create an OpcMonitoredItem for the NodeId.
                var item = new OpcMonitoredItem(nodeIds[index], OpcAttribute.Value);
                item.DataChangeReceived += HandleDataChanged;
                // You can set your own values on the "Tag" property
                // that allows you to identify the source later.
                item.Tag = index;

                // Set a custom sampling interval on the 
                // monitored item.
                item.SamplingInterval = 1;
                // Add the item to the subscription.
                subscription.AddMonitoredItem(item);
            }
            subscription.ApplyChanges();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMicroseconds(1); // Set the interval to 1 second
            timer.Tick += Timer_Tick;
            timer.Start();

        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update your UI or perform any other operation here
            // For demonstration, let's update a label with the current time
            tagTemp.Text =  (count.ToString() + "    ").Substring(0,2) + " " + vall;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //string nodeurl = "opc.tcp://ALEXHOME:26543";
            //string tagname = "Objects/ProcessModule/Temperature";
            string tagname = "ns=1;i=1001";
            //client
            //client = new client(nodeurl);
            if (client.State != OpcClientState.Connected)
            {
                client.Connect();
            }
            //var temperature = none;// = client.ReadNode(tagname);  // = "Temperature";
            var temperature = client.ReadNode(tagname);  // = "Temperature";
            //var temperature = client.ReadNode(1,1001);  // = "Temperature";
            this.tagTemp.Text = temperature.ToString();
        }
    }
}