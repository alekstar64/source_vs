using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace WpfOOP_Test
{
    public class Item
    {
        //private string _name = "";
        public string Name { get; set; }
    }
    

    public class Folder<list> :Item
    {
        //private string _name = "";
        public string FName { get; set; }
    }
    public class Root<Folder> 
    { }
    //class Item
    //{
    //    private string _name = "";

    //    public string Name
    //    {
    //        get { return _name; }
    //        set { _name = value; }
    //    }
    //    public void Print()
    //    {
    //        Console.WriteLine(Name);
    //    }
    //}
    //class Employee : Item
    //{

    //}
    //public class Item
    //{
    //    public string Name { get; set; }
    //    public string Link { get; set; }
    //    public string Description { get; set; }
    //}
    //public class Groups_Folder : List<Group_Folder>
    //{
    //    public void Add(string _name, string _description)
    //    {
    //        this.Add(new Groups_Folder(_name, _description));
    //    }
    //}
    //public class Groups_Folder
    //{
    //    string name;
    //    string description;
    //    public Groups_Folder(string _name, string _description)
    //    {
    //        name = _name;
    //        description = _description;
    //    }
    //}
}
