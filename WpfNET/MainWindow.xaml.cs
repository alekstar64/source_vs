using System.Windows;
using System.Windows.Input;

namespace WpfNET
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public Tree tree = new Tree("Root");
        private void Main_Loaded(object sender, RoutedEventArgs e)
        {
            
            //Public static void Main()
            //Tree tree = new Tree("Root");
            // Add folders into ROOT
            tree.AddFolder(tree.Root, "Folder1");//Foder_ID = 1
            tree.AddFolder(tree.Root, "Folder2");//Foder_ID = 2
            tree.AddFolder(tree.Root, "Folder3");//Foder_ID = 3
            // Add subfolders into Folder where Folder.Foder_ID = 1
            var f4 = tree.AddFolder((Folder) tree.GetFolderByIndex(1, tree.Root), "subfolder");//Foder_ID = 4
            // Add item into subfolder
            tree.AddItem(f4, "Item2");
            // or tree.AddItem(tree.GetFolderByIndex(4, tree.Root), "Item2");
            // Add items into root
            tree.AddItem(tree.Root, "Item1");
            // Add item into subfolder
            tree.AddItem(tree.GetFolderByIndex(4, tree.Root), "Item2_1");
            // Search
            //var searchResults = tree.Search("Item", tree.Root);
            refresh_list();
            PrnList.ToolTip = "Structure of string: \r\n";
            PrnList.ToolTip += "1 - Leading spaces depend on hierarchy level \r\n";
            PrnList.ToolTip += "2 - Symvol F/I = Item/Foolder + Symbol_space \r\n";
            PrnList.ToolTip += "3 - numeric ID = Item/Foolder identifier + Symbol_space \r\n";
            PrnList.ToolTip += "4 - Text = Object Name";
        }
        // Fill out information about object into input fields from Object List
        private void PrnList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string[] ret =  PrnList.SelectedItem.ToString().Trim().Split(' ');
            this.txt_id.Text = ret[1];
            this.txt_name.Text = "";
            for (int i = 2; i < ret.Length; i++)
            {
                this.txt_name.Text += ret[i] + " ";
            }
            this.txt_name.Text = this.txt_name.Text.Trim();
            switch (ret[0])
            {
                case "F": this.txt_obj_type.Text = "Folder";
                    break;
                case "I": this.txt_obj_type.Text = "Item";
                    break;
                default:  this.txt_obj_type.Text = "";
                    break;
            }
        }
        // Deleting item
        private void cmd_delete_item_Click(object sender, RoutedEventArgs e)
        {
            string txt_id = this.txt_id.Text;
            Item item = new Item();
            if (! int.TryParse(txt_id, out int id) || (tree.GetItemByIndex(id, tree.Root) is null)) 
            {
                //wromg int or object ID
                MessageBox.Show("Incorrect source object index", "Data error...");
                return;
            }
            item = tree.GetItemByIndex(id, tree.Root);
            if (((string)this.txt_name.Text).Trim() == "")
            {
                //empty fieldName
                MessageBox.Show("Fill in the object name field", "Data error...");
                return;
            }
            tree.DeleteItem(tree.GetItemByIndex(id, tree.Root));
            refresh_list();
        }

        private void cmd_add_Item_Click(object sender, RoutedEventArgs e)
        {
            //int id = -1;
            string txt_id = this.txt_dest_id.Text;
            if (! int.TryParse(txt_id, out int id) || tree.GetFolderByIndex(id, tree.Root) is null)
            {
                //wromg int or object ID
                MessageBox.Show("Incorrect destination index number ", "Data error...");
                return;
            }
            if (((string)this.txt_name.Text).Trim() == "")
            {
                //empty fieldName
                MessageBox.Show("Fill in the object name field", "Data error...");
                return;
            }
            tree.AddItem(tree.GetFolderByIndex(id, tree.Root), ((string)this.txt_name.Text).Trim());
            refresh_list();
        } 
        // Output objects information
        public void refresh_list()
        {
            this.PrnList.Items.Clear();
            var rel_rep = tree.PrintList(tree.Root, 0);            
            foreach (var str in rel_rep)
            {
                PrnList.Items.Add(str);
            }
        }
        // Moving Item 
        private void cmd_move_item_Click(object sender, RoutedEventArgs e)
        {
            {
                string txt_id = this.txt_id.Text;
                string txt_dest_id = this.txt_dest_id.Text;
                if (!int.TryParse(txt_id, out int id) || tree.GetItemByIndex(id, tree.Root) is null)
                {
                    //wromg int for Item ID
                    MessageBox.Show("Incorrect source index number ", "Data error...");
                    return;
                }
                Item item = new Item();
                item = tree.GetItemByIndex(id, tree.Root);
                if (!int.TryParse(txt_dest_id, out int dest_id) || tree.GetFolderByIndex(dest_id, tree.Root) is null)
                {
                    //wromg int for destination ID
                    MessageBox.Show("Incorrect destination index number ", "Data error...");
                    return;
                }
                tree.Move(item, tree.GetFolderByIndex(dest_id, tree.Root));
                refresh_list();
            }
        }
        // Movig folder action
        private void cmd_move_Folder_Click(object sender, RoutedEventArgs e)
        {
            {
                Item item = new Item();
                string txt_id = this.txt_id.Text;
                string txt_dest_id = this.txt_dest_id.Text;
                if (!int.TryParse(txt_id, out int id) || (tree.GetFolderByIndex(id, tree.Root) is null))
                {
                    //wromg int
                    MessageBox.Show("Incorrect source index number ", "Data error...");
                    return;
                }
                if (!int.TryParse(txt_dest_id, out int dest_id) || (tree.GetFolderByIndex(dest_id, tree.Root) is null))
                {
                    //wromg int or destination folder ID
                    MessageBox.Show("Incorrect destination index number ", "Data error...");
                    return;
                }
                tree.Move(tree.GetFolderByIndex(id, tree.Root), tree.GetFolderByIndex(dest_id, tree.Root));
                refresh_list();
            }
        }
        // deleteng folder
        private void cmd_delete_folder_Click(object sender, RoutedEventArgs e)
        {
            string txt_id = this.txt_id.Text;
            Folder folder = new Folder();
            if (!int.TryParse(txt_id, out int id) || (tree.GetFolderByIndex(id, tree.Root) is null))
            {
                //wromg int or destinetion folder
                MessageBox.Show("Incorrect source folder number or ID", "Data error...");
                return;
            }
            folder = tree.GetFolderByIndex(id, tree.Root);
            tree.DeleteFolder(tree.GetFolderByIndex(id, tree.Root));
            refresh_list();
        }
        // adding Folder action
        private void cmd_add_folder_Click(object sender, RoutedEventArgs e)
        {
            string txt_id = this.txt_dest_id.Text;
            if (!int.TryParse(txt_id, out int id) || (tree.GetFolderByIndex(id, tree.Root) is null))
            {
                //wromg int or destinetion index 
                MessageBox.Show("Incorrect destinetion index number ", "Data error...");
                return;
            }
            if (((string)this.txt_name.Text).Trim() == "")
            {
                //empty fieldField
                MessageBox.Show("Fill in the object name field", "Data error...");
                return;
            }
            tree.AddFolder(tree.GetFolderByIndex(id, tree.Root), ((string)this.txt_name.Text).Trim());
            refresh_list();
        }
    }
}
