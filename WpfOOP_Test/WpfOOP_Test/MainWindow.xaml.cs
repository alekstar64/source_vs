using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfOOP_Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //List foder<Folder> = new Foder;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Main_Loaded(object sender, RoutedEventArgs e)
        {
            //List Foder = new Foder<List>Foder
            //Foder.
            Item item = new Item();
            item.Name = "Test";
            //item.Name = "Test";
        }


        public class Foder<list>
        {
            public string Name { get; set; }
            public string Range { get; set; }
            public string Description { get; set; }
            public void New()
            {

            }
        }
    }
}
