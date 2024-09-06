using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;


namespace WpfWSS_CS
{
    public class WSCl
    {
        public WebSocket Socket { get; set; }
        public int id { get; set; }
        // Дополнительные свойства и методы для обработки клиента
    }

    // WSClients.cs
    public class WSClients : CollectionBase
    {
        public void Add(WSCl client)
        {
            client.id = this.Count;
            List.Add(client);            
        }

        public void Remove(WSCl client)
        {
            List.Remove(client);
        }

        public WSCl this[int index]
        {
            get => (WSCl)List[index];
            set { List[index] = value; }
        }
    }

    public class WebSocketServer
    {
        private readonly WSClients clients = new WSClients();

        public async Task StartAsync(int port)
        {
            var listener = new HttpListener();
            listener.Prefixes.Add($"http://localhost:{port}/");
            listener.Start();

            while (true)
            {
                var context = await listener.GetContextAsync();
                if (context.Request.IsWebSocketRequest)
                {
                    var webSocketContext = await context.AcceptWebSocketAsync(null);
                    var socket = webSocketContext.WebSocket;

                    // Создаем новый экземпляр WSCl для клиента и добавляем его в список клиентов
                    var client = new WSCl { Socket = socket };
                    clients.Add(client);

                    // Запускаем обработку клиента в отдельном потоке
                    Task.Run(() => HandleClient(client));
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
            }
        }

        private async Task HandleClient(WSCl client)
        {
            var socket = client.Socket;
            var buffer = new byte[1024];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    // Обработка входящего текстового сообщения
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    // Отправка ответа клиенту
                    await SendTextAsync(client, "Received: " + message);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    clients.Remove(client);
                }
            }
        }

        public async Task SendTextAsync(WSCl client, string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            await client.Socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }

}

