using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNET
{
    internal class Garbage
    {

        // Move
        //     tree.Move(searchResults[0], folder2);

        // Delete
        //    tree.DeleteFolder(folder2);

        //TreeView root = new TreeView();


        //TreeView treeView = new TreeView();
        //Folder folder = new Folder();
        //Item item = new Item();
        //folder.Contents.Add(item);
        ////folder.Contents.Add(item);
        //treeView.AddItem(folder, item);
        ////treeView.AddFolder(treeView, folder);
        //item.Name = "Item";
        //  treeView.
        /*
        var item1 = new Item { Name = "Item 1" };
        var item2 = new Item { Name = "Item 2" };
        root.AddItem(item1);
        root.AddItem(item2);

        // Adding a sub-folder to the root folder
        var Folder = new Folder { Name = "folder_ROOT" };
        var subFolder = new Folder { Name = "Sub-folder" };
        Folder.Move(subFolder);
        root.
        root.AddFolder(root,Folder);
        root.AddFolder(subFolder);
        */


        /*
        //     var root = new Folder { Name = "Root" };

        // Adding items to the root folder
        var item1 = new Item { Name = "Item 1" };
        var item2 = new Item { Name = "Item 2" };
        root.AddItem(item1);
        root.AddItem(item2);

        // Adding a sub-folder to the root folder
        var Folder = new Folder { Name = "folder_ROOT" };
        var subFolder = new Folder { Name = "Sub-folder" };
        Folder.Move(subFolder);
        root.AddFolder(Folder);
        root.AddFolder(subFolder);
        // Moving item1 to the sub-folder
        //subFolder.Move(Folder);
        //root.Children.
        Folder.Move(subFolder);

        // Searching for items matching the keyword "Item"
        var matches = root.Search("Item");
        foreach (var match in matches)
        {
            Console.WriteLine(match.Name);
        }

        // Deleting the root folder (and all contained items and folders)
        root.Delete();
        */

        /*
        //            Folder root = new Folder("Root");

                  // Add folders
                  var folder1 = new Folder("Folder1");
                  root.Add(folder1);
                  var INfolder = new Folder("INFolder");

                  folder1.Add(INfolder);
                  // Add items
                  var item1 = new Item("Item1");
                  root.Add(item1);

                  var item2 = new Item("Item2");
                  folder1.Add(item2);


                  var Infolder1 = new Folder("InFolder1");
                  folder1.Add(Infolder1);
                  // Search
                  var searchResults = root.Search("Item");
                  Console.WriteLine($"Search results count: {searchResults.Count}");

                  // Move
                  folder1.Add(item1);
                  root.Remove(item1);

                  // Delete
                  root.Remove(folder1);
                  */
        /*            
                    var root = new Folder("Root", 0);

                    // Adding items to the root folder
                    var item1 = new Item("Item 1", 1);
                    root.AddItem(item1,0);
                    var folder1 = new Folder("Folder1",1);
                    root.AddFolder(folder1, 0);
                    var folderIN1 = new Folder("folderIN1", 1);
                    folder1.AddFolder(folderIN1, 0);
                    folderIN1.AddItem(item1, 0);
                    folderIN1.AddItem(item1);
                    int i = 0;
        */
        /*    _Folder rroot = new _Folder();
            List<_Item> _Items = new List<_Item> { };
            //_Items.Items.Count = 0;
            _Item _Item = new _Item ("Interf");
            //_Item.IName = "ss";
            _Items.Add(_Item);

            //rroot._Items.Add( _Items);


           LItem lItem = new LItem();
            //lItem.
            Item item = new Item { };
            item.IName = "Item";
            ListOfItem listOfItem = new ListOfItem();
            //listOfIten = null; 
            listOfItem.ILName = "Item";
            List<Item> items = new List<Item>();
            listOfItem.items = items;
            listOfItem.items.Add(item);
            item = new Item { };
            item.IName = "Item2";
            listOfItem.items.Add(item);
            _root root = new _root { };
            root.folder_items = new List<ListOfItem> { listOfItem }; 
            root.folder_items.Add( listOfItem);


            //listOfIten
            //listOfIten.item.add(item);
            //     listOfIten listOfIten = new ListOfIten<List>;
            //   listOfIten.LOI.Add(item);
            listOfItem.ILName = "ILNAME";



            //listOfIten

            string q = string.Empty;
            //Item item = new Item { };
            //item.Name = "Item";
            //   Root<List> root = new Root<List>;
        */
        /*
         * 
        public class Node
        {
            public string Name { get; set; }
        }

        public class Folder : Node
        {
            public List<Node> Contents { get; set; } = new List<Node>();
        }

        public class Item : Node { }

        public class TreeView
        {
            public Folder Root { get; } = new Folder();

            public void AddItem(Folder folder, Item item)
            {
                folder.Contents.Add(item);
            }

            public void DeleteItem(Folder folder, Item item)
            {
                folder.Contents.Remove(item);
            }

            public void AddFolder(Folder parentFolder, Folder newFolder)
            {
                parentFolder.Contents.Add(newFolder);
            }

            public void DeleteFolder(Folder parentFolder, Folder folderToDelete)
            {
                parentFolder.Contents.Remove(folderToDelete);
            }

            public void MoveNode(Folder sourceFolder, Node node, Folder targetFolder)
            {
                sourceFolder.Contents.Remove(node);
                targetFolder.Contents.Add(node);
            }

            public List<Node> Search(Node startNode, string searchTerm)
            {
                List<Node> results = new List<Node>();

                if (startNode.Name.Contains(searchTerm))
                {
                    results.Add(startNode);
                }

                if (startNode is Folder folder)
                {
                    foreach (var content in folder.Contents)
                    {
                        results.AddRange(Search(content, searchTerm));
                    }
                }

                return results;
            }

            public List<Node> GetAllItemsOfType<T>()
            {
                List<Node> results = new List<Node>();

                GetAllItemsOfType(Root, typeof(T), results);

                return results;
            }

            private void GetAllItemsOfType(Folder folder, Type targetType, List<Node> results)
            {
                foreach (var content in folder.Contents)
                {
                    if (content.GetType() == targetType)
                    {
                        results.Add(content);
                    }

                    if (content is Folder subfolder)
                    {
                        GetAllItemsOfType(subfolder, targetType, results);
                    }
                }
            }
        }
        }
        */
        /*
        public class Node
        {
            public string Name { get; set; }
            public List<Node> Children { get; set; }

            public Node(string name)
            {
                Name = name;
                Children = new List<Node>();
            }
            public void AddItem(string itemName)
            {
                Children.Add(new Node(itemName));
            }

            public void DeleteItem(string itemName)
            {
                Node item = Children.FirstOrDefault(child => child.Name == itemName);
                if (item != null)
                {
                    Children.Remove(item);
                }
            }
            public void AddFolder(string folderName)
            {
                Children.Add(new Node(folderName));
            }

            public void DeleteFolder(string folderName)
            {
                Node folder ==> Children.Name == Name);
                if ( != null)
                {
               );
                }
            }
            public void MoveNode(Node sourceNode, Node targetNode)
            {
                targetNode.Children.Add(sourceNode);
                Children.Remove(sourceNode);
            }
            public IEnumerable<Node> Search(string searchTerm)
            {
                if (Name.Contains(searchTerm))
                {
                    yield return this;
                }
                foreach (var child in Children)
                {
                    foreach (var match in child.Search(searchTerm))
                    {
                        yield return match;
                    }
                }
            }
            public IEnumerable<Node> GetAllItemsOfType(string itemType)
            {
                if (Children.Count == 0 && Name == itemType)
                {
                    yield return this;
                }
                foreach (var child in Children)
                {
                    foreach (var item in child.GetAllItemsOfType(itemType))
                    {
                        yield return item;
                    }
                }
            }

        }

        }
        */
        /*
        public class Node
        {
            public string Name { get; set; }
        }

        public class Folder : Node
        {
            public List<Node> Contents { get; set; } = new List<Node>();
        }

        public class Item : Node { }

        public class TreeView
        {
            public Folder Root { get; } = new Folder();

            public void AddItem(Folder folder, Item item)
            {
                folder.Contents.Add(item);
            }

            public void DeleteItem(Folder folder, Item item)
            {
                folder.Contents.Remove(item);
            }

            public void AddFolder(Folder parentFolder, Folder newFolder)
            {
                parentFolder.Contents.Add(newFolder);
            }

            public void DeleteFolder(Folder parentFolder, Folder folderToDelete)
            {
                parentFolder.Contents.Remove(folderToDelete);
            }

            public void MoveNode(Folder sourceFolder, Node node, Folder targetFolder)
            {
                sourceFolder.Contents.Remove(node);
                targetFolder.Contents.Add(node);
            }

            public List<Node> Search(Node startNode, string searchTerm)
            {
                List<Node> results = new List<Node>();

                if (startNode.Name.Contains(searchTerm))
                {
                    results.Add(startNode);
                }

                if (startNode is Folder folder)
                {
                    foreach (var content in folder.Contents)
                    {
                        results.AddRange(Search(content, searchTerm));
                    }
                }

                return results;
            }

            public List<Item> GetAllItems()
            {
                List<Item> items = new List<Item>();
                GetAllItems(Root, items);
                return items;
            }

            private void GetAllItems(Folder folder, List<Item> items)
            {
                foreach (var content in folder.Contents)
                {
                    if (content is Item item)
                    {
                        items.Add(item);
                    }

                    if (content is Folder subfolder)
                    {
                        GetAllItems(subfolder, items);
                    }
                }
            }
        }
        }
        */
        /*
            public class Node
            {
                public string Name { get; set; }
            }

            public class Folder : Node
            {
                public List<Node> Children { get; set; }

                public Folder()
                {
                    Children = new List<Node>();
                }

                public void AddItem(Item item)
                {
                    Children.Add(item);
                }

                public void AddFolder(Folder folder)
                {
                    Children.Add(folder);
                }

                public void Delete()
                {
                    Children.Clear();
                }

                public void Move(Folder destination)
                {
                    destination.Children.AddRange(Children);
                    Children.Clear();
                }

                public List<Node> Search(string keyword)
                {
                    List<Node> matches = new List<Node>();
                    foreach (var child in Children)
                    {
                        if (child is Folder folder)
                        {
                            matches.AddRange(folder.Search(keyword));
                        }
                        if (child.Name.Contains(keyword))
                        {
                            matches.Add(child);
                        }
                    }
                    return matches;
                }
            }

            public class Item : Node
            {
                // Item properties if needed
            }

            public class TreeView
            {
                public Folder RootFolder { get; set; }

                public TreeView()
                {
                    RootFolder = new Folder();
                }

                public List<Node> Search(string keyword)
                {
                    return RootFolder.Search(keyword);
                }
            }



            */

        /*  
          public abstract class Node
              {
                  public string Name { get; set; }                      
                  public Folder Parent { get; set; } 
                  protected Node(string name)
                  {
                      Name = name;
                  }

                  public abstract List<Node> Search(string keyword);
              }

              public class Folder : Node
              {
                  public int folder_cout { get; set; } = 0;
                  public List<Node> Children { get; private set; }
                  public int Level { get; set; }
                  public Folder(string name) : base(name)
                  {
                      Children = new List<Node>();
                  }

                  public void Add(Node node)
                  {
                      node.Parent = this;                
                      Children.Add(node);
                  }

                  public void Remove(Node node)
                  {
                      if (Children.Contains(node))
                      {
                          Children.Remove(node);
                          node.Parent = null;
                      }
                  }

                  public override List<Node> Search(string keyword)
                  {
                      List<Node> results = new List<Node>();

                      foreach (var child in Children)
                      {
                          if (child.Name.Contains(keyword))
                              results.Add(child);
                          //child.GetHashCode

                          if (child is Folder folder)
                              results.AddRange(folder.Search(keyword));
                      }

                      return results;
                  }
              }

              public class Item : Node
              {
                  public int item_cout { get; set; } = 0;
                  public Item(string name) : base(name)
                  {
                  }

                  public override List<Node> Search(string keyword)
                  {
                      return Name.Contains(keyword) ? new List<Node> { this } : new List<Node>();
                  }
              }
        */
        /*  
          public abstract class Node
          {
              public string Name { get; set; }
              public Folder Parent { get; set; }

              protected Node(string name, int level)
              {
                  Name = name;
                  Level = level;
              }

              public abstract List<Node> Search(string keyword);
          }

          public class Folder : Node
          {
              public List<Node> Children { get; private set;}

              public Folder(string name) : base(name)
              {
                  Children = new List<Node>();            
              }

              public void AddItem(Item item)
              {
                  Children.Add(item);
              }

              public void AddFolder(Folder folder)
              {
                  Children.Add(folder);
              }

              public void Delete()
              {
                  Children.Clear();
              }

              public void Move(Folder destination)
              {
                  destination.Children.AddRange(Children);
                  Children.Clear();
              }

              public override List<Node> Search(string keyword)
              {
                  List<Node> matches = new List<Node>();
                  foreach (var child in Children)
                  {
                      if (child is Folder folder)
                          matches.AddRange(folder.Search(keyword));

                      if (child.Name.Contains(keyword))
                          matches.Add(child);
                  }
                  return matches;
              }
          }

          public class Item : Node
          {
              public Item(string name) : base(name)
              {
              }

              public override List<Node> Search(string keyword)
              {
                  return new List<Node>();
              }
          }

          public class TreeView
          {
              public Folder RootFolder { get; set; }

              public TreeView()
              {
                  RootFolder = null;
              }

              public void SetRootFolder(Folder rootFolder)
              {
                  RootFolder = rootFolder;
              }

              public List<Node> Search(string keyword)
              {
                  if (RootFolder == null)
                      return new List<Node>();

                  return RootFolder.Search(keyword);
              }
          }

        */

        /*
        public class _Items : List
        {
            public void Add(_Item _Item)
            {
                this.Add(_Item);
            }

            public List<_Item> Items { get; set; }
        }
        public class _Item 
        {
            public _Item (string Name)
            {
                IName = Name;
            }
            public string IName { get; set; }
        }


        public class CustomerListList : List<CustomerList> { }

        public class CustomerList : List<Customer> { }

        public class Customer
        {
            public int ID { get; set; }
            public string SomethingWithText { get; set; }
        }


        public class _Folder : List
        {
            public List<_Items> _Items { get; set; }
            public List<_Folder> _Folders { get; set; }
            public void ItemsAdd(List<_Items> _Items)
            {
                this.Add(_Items);
            }
            public void FolderAdd(List<_Folder> _Folders)
            {
                //this._Folders.Add(_Folders)
            }
        }



        public class LItem : List
        {
            public string IName { get; set; }
            public string Description { get; set; }
         //   public void Invoke() { }
        }

        public class _root : ListOfFolder<List>
        {
            public List<ListOfItem> folder_items { get; set; }

            //public string Name { get; set; } = "root";
            //public string Description { get; set; } = "root";
            //public ListOfFolder L_Forder { get; set; };
            //public ListOfIten<List> L_Item { get; set; } ;
        }
        public class Item
        {
            public string IName { get; set; }
            public string Description { get; set; }
            public void Invoke() { }
        }
        public class ListOfItem
        {
            public string ILName { get; set; }
            public string Description { get; set; }
            public List<Item> items { get; set; }
         //   public void Invoke()          {            //items = new List<Item>();            //List<Item> items = new List<Item>();            //items = items;
           // }
        }

        public class Folder
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public List<Folder> Folders { get; set; }
        }

        public class ListOfFolder<List> //: Folder<List>
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }
        //public class _Root<List> : ListOfFolder<List>
        //{
        //    //public string Name { get; set; } = "root";
        //    //public string Description { get; set; } = "root";
        //    //public ListOfFolder L_Forder { get; set; };
        // //   public ListOfIten<List> L_Item { get; set; } : ;
        //}

        */

    }
}
