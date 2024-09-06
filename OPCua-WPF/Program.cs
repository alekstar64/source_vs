using Opc.UaFx.Client;
using System;
using System.Threading.Tasks;

class Program
{
    //static async Task Main(string[] args)
    //{
    //    string nodeurl = "opc.tcp://ALEXHOME:26543";
    //    string tagname = "ns=1;i=1001";
    // //   string[] nodeIds = {"ns=1;i=1001"};

    //    // Создание клиента OPC UA
    //    using (var client = new OpcClient(nodeurl))
    //    {
    //        await client..ConnectAsync();

    //        // Создание группы тегов
    //        var group = client.SubscribeNodesGroup();

    //        // Добавление тега в группу
    //        var tagNode = group.AddNode(tagname);

    //        // Обработчик события изменения значения тега
    //        tagNode.ValueChanged += (sender, e) =>
    //        {
    //            Console.WriteLine($"Значение тега {tagname} изменено на: {e.Value}");
    //        };

    //        Console.WriteLine("Нажмите Enter для завершения работы...");
    //        Console.ReadLine();
    //    }
   // }
}