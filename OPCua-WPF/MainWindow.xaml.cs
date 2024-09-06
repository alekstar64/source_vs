using Opc.UaFx;
using Opc.UaFx.Client;
using Org.BouncyCastle.Asn1.X509;
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

namespace OPCua_WPF
{

    
    public partial class MainWindow : Window
    {
        private static void HandleDataChanged(
        object sender,
        OpcDataChangeReceivedEventArgs e)
        {
            // The tag property contains the previously set value.
            OpcMonitoredItem item = (OpcMonitoredItem)sender;
            //tagTemp.Text = e.Item.Value;
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
                item.SamplingInterval = 200;

                // Add the item to the subscription.
                subscription.AddMonitoredItem(item);
            }

            // After adding the items (or configuring the subscription), apply the changes.
            subscription.ApplyChanges();
        }
        public OpcClient client = new OpcClient(nodeurl);
        public static string nodeurl = "opc.tcp://ALEXHOME:26543";
        public static string[] nodeIds = { "ns=1;i=1001" };
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //string nodeurl = "opc.tcp://ALEXHOME:26543";
            //string tagname = "Objects/ProcessModule/Temperature";
            string tagname = "ns=1;i=1001";
            //client
            //client = new client(nodeurl);
            if (client.State != OpcClientState.Connected )
            {
                client.Connect();
            } 
            //var temperature = none;// = client.ReadNode(tagname);  // = "Temperature";
            var temperature = client.ReadNode(tagname);  // = "Temperature";
            //var temperature = client.ReadNode(1,1001);  // = "Temperature";
            //this.tagTemp.Text = temperature.ToString();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string nodeurl = "opc.tcp://ALEXHOME:26543";
            //string tagname = "Objects/ProcessModule/Temperature";
            string tagname = "ns=1;i=1001";
            var client = new OpcClient(nodeurl);
            client.Connect();
            double temperature = Convert.ToDouble( this.TagTemp.Text);// = client.ReadNode(tagname);  // = "Temperature";
            //var temperature = client.ReadNode(tagname);  // = "Temperature";
            client.WriteNode(tagname, temperature);
            //var temperature = client.ReadNode(1,1001);  // = "Temperature";
           // this.TagTemp.Text = temperature.ToString();

        }
    }
}