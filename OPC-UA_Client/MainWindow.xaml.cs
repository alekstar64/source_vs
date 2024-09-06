using System;
using System.Threading.Tasks;
using Opc.Ua;
using Opc.Ua.Client;
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

namespace OPC_UA_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>



        public MainWindow()
        {
            InitializeComponent();



        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string opcUrl = "opc.tcp://alexhome:62555/SampleServer";
            //string opcUrl = "opc.tcp://alexhome:62555";
            string opcTag = "SystemCycleStatusEventType_LocalTime";
            //var client = new Opc.Ua.Client();
            //client.Connect();
            //var mes = client.ReadNode(opcTag);
            //this.txtOPC.Text = mes.ToString();




        }
    
} 