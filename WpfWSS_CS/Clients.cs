using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace WpfWSS_CS
{
    using System;
    using System.Collections;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Text.RegularExpressions;

    public class Clients : CollectionBase
    {
        public Socket? proxySocket;
        public Socket? serverSocket;
        public Socket? clientSocket;

        public Clients()
        {
            this.Remove_ALL();
        }

        public void Send_Image(string base64)
        {
            try
            {
                for (short i = 0; i < this.List.Count; i++)
                {
                    if (((Client)this[i]).IsActive)
                    {
                        ((Client)this[i]).TEXTBOX_BUFF = base64;
                    }
                }
            }
            catch (Exception )
            {
                // Handle exception
            }
        }

        public void NewSocket(int p_Server_Port)
        {
            if (serverSocket == null)
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint IpEndPoint = new IPEndPoint(IPAddress.Any, p_Server_Port);

                try
                {
                    serverSocket.Bind(IpEndPoint);
                    serverSocket.Listen(50);
                }
                catch (Exception )
                {
                    // Handle exception
                }

                try
                {
                    serverSocket.BeginAccept(new AsyncCallback(OnAccept), null);
                }
                catch (Exception )
                {
                    // Handle exception
                }
            }
        }

        public void Connect_Proxy(IPAddress p_IPAddress, int p_Server_Port)
        {
            if (serverSocket == null)
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint IpEndPoint = new IPEndPoint(p_IPAddress, p_Server_Port);

                try
                {
                    serverSocket.Connect(IpEndPoint);
                }
                catch (Exception )
                {
                    // Handle exception
                }

                try
                {
                    serverSocket.BeginAccept(new AsyncCallback(OnAccept), null);
                }
                catch (Exception )
                {
                    // Handle exception
                }
            }
        }

        private void OnAccept(IAsyncResult ar)
        {
            if (ar == null)
            { 
                return;
            }
                try
            {
                
                    clientSocket = serverSocket.EndAccept(ar);
                
            }
            catch (Exception ex)
            {
                add_mod.p_errlog("Sub_OnAccept", "serverSocket.EndAccept(ar)", ex.Message);
                return;
            }

            serverSocket.BeginAccept(new AsyncCallback(OnAccept), null);
            AddClient(clientSocket);
        }

        private void AddClient(Socket client)
        {
            if (client.Connected)
            {
                byte[] bytes = new byte[1024];
                int byteCount = client.Receive(bytes, 0, bytes.Length, SocketFlags.None);
                string data = Encoding.UTF8.GetString(bytes);
                Regex regex = new Regex("");
                if (System.Text.RegularExpressions.Regex.IsMatch(data, "^GET"))
                {
                    byte[] response = Encoding.UTF8.GetBytes("HTTP/1.1 101 Switching Protocols" +
                       Environment.NewLine + "Connection: Upgrade" +
                       Environment.NewLine + "Upgrade: websocket" +
                       Environment.NewLine + "Sec-WebSocket-Accept: " +
                       Convert.ToBase64String(System.Security.Cryptography.SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(new Regex("Sec-WebSocket-Key: (.*)").Match(data).Groups[1].Value.Trim() + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"))) +
                       Environment.NewLine + Environment.NewLine);
                    client.Send(response, response.Length, SocketFlags.None);
                }
                if (client == null)
                {
                    return;
                }
                short i = (short) HttpAll.F_ADD_CLIENT(client.RemoteEndPoint.ToString(), client);

                if (i < 0)
                {
                    return;
                }
            }
        }

        public void Add(Client value)
        {
            List.Add(value);
        }

        public bool Contains(Client value)
        {
            return List.Contains(value);
        }

        public int IndexOf(Client value)
        {
            return List.IndexOf(value);
        }

        public void Insert(int index, Client value)
        {
            List.Insert(index, value);
        }

        public new int Count
        {
            get { return List.Count; }
        }

        public Client this[int index]
        {
            get
            {
                try
                {
                    return (Client)List[index];
                }
                catch (Exception ex)
                {
                    add_mod.p_errlog("Clients_Item", "Return DirectCast(List.Item(index), Client)", ex.Message);
                    return null;
                }
            }
        }

        public void Remove(int value)
        {
            List.RemoveAt(value);
        }

        public void Remove_ALL()
        {
            while (List.Count != 0)
            {
                ((Client)List[0]).Socket.Dispose();
                ((Client)List[0]).Socket.Close();
                List.RemoveAt(0);
            }
        }
    }
}
