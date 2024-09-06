using System;
using System.Collections.Generic;

namespace WpfNET
{
    // Basic classes
    public abstract class Node
    {
        public string Name { get; set; } = "";
        public Folder Parent { get; set; } 
        // Base object identifiers 
        public int folder_id { get; set; } = -1;
        public int item_id { get; set; } = -1;
    }
    // Folder object
    public class Folder : Node
    {
        public List<Node> Children { get; set; }
        public Folder()
        {
            Children = new List<Node>();
        }
    }
    // Item object
    public class Item : Node
    { 
    
    }
    // Main object Tree
    public class Tree
    {
        public Folder Root { get; private set; }
        // Sequence/counter to for base object identifiers 
        public int folder_index { get; set; }
        public int item_index { get; set; }
        // Initialising Root methods
        public Tree(string rootName)
        {
            Root = new Folder { Name = rootName };
            Root.folder_id = 0;
            folder_index = 0;
        }
        // Add Item function
        public Item AddItem(Folder parent, string itemName)
        {
            var item = new Item { Name = itemName, Parent = parent };
            item_index += 1;
            item.item_id += item_index;
            parent.Children.Add(item);
            return item;
        }
        // Delete Item method
        public void DeleteItem(Item item)
        {
            item.Parent.Children.Remove(item);
        }
        // Add Folder function
        public Folder AddFolder(Folder parent, string folderName)
        {
            var folder = new Folder { Name = folderName, Parent = parent };
            folder_index += 1;
            folder.folder_id = folder_index;
            parent.Children.Add(folder);
            return folder;
        }
        // Delete Folder method including Children objects
        public void DeleteFolder(Folder folder)
        {
            // Hanling for root
            if (folder.folder_id == 0)
            {
                folder.Children.Clear();
                return;
            }
            folder.Parent.Children.Remove(folder);
        }
        // Moving objects method 
        public void Move(Node node, Folder newParent)
        {
            node.Parent.Children.Remove(node);
            node.Parent = newParent;
            newParent.Children.Add(node);
        }
        // Searchig function by Item Name. Not used for this project
        public List<Node> Search(string name, Folder startFolder)
        {
            List<Node> results = new List<Node>();
            foreach (var child in startFolder.Children)
            {
                if (child.Name.Contains(name))
                    results.Add(child);
                if (child is Folder childFolder)
                    results.AddRange(Search(name, childFolder));
            }
            return results;
        }
        // Additional searching function - Get Item By Index
        public Item GetItemByIndex(int index, Folder startFolder)
        {
            Item ret = new Item();
            foreach (var child in startFolder.Children)
            {
                if (child is Item && child.item_id == index)
                    return (Item)child;
                if (child is Folder childFolder)
                {
                    ret = GetItemByIndex(index, childFolder);
                    if (ret is Item && ret.item_id == index)
                        return (Item)ret;
                }
            }
            return ret;
        }
        // Additional searching function - Get Folder By Index
        public Folder GetFolderByIndex(int index, Folder startFolder)
        {
            Folder  ret = new Folder();
            if (index == 0)
                return startFolder;
            foreach (var child in startFolder.Children)
            {
                if (child is Folder && child.folder_id == index)
                    return  (Folder)child;
                if (child is Folder childFolder)
                {
                    ret = GetFolderByIndex(index, childFolder);
                    if (ret is Folder && ret.folder_id == index)
                        return (Folder)ret;
                }
            }
            return ret;
        }
        //Function is returning curren Tree in List<string>
        //Structure of string:
        //1 - Leading spaces depend on hierarchy level
        //Symvol F/I = Item/Foolder + Symvol_space
        //numeric ID = Item/Foolder identifier + Symvol_space
        //Text = Object Name
        public List<string> PrintList(Folder startFolder,int level)
        {
            List<string> results = new List<string>();
            string str;
            if (level == 0)
                results.Add("F 0 ROOT");
            level ++;
            foreach (var child in startFolder.Children)
            {
                if (child is Folder childFolder) 
                {
                    str = new String(' ', level * 4) + "F " + child.folder_id + " " + child.Name;
                    results.Add(str);
                    results.AddRange(PrintList(childFolder, level + 1));
                }
                else
                {
                    str = new String(' ', level * 4) + "I " + child.item_id + " " + child.Name;
                    results.Add(str);                    
                }
            }
            return results;
        }
    }
}






