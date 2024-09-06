namespace Blazor_WSS_CS
{
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Runtime.InteropServices;

    public class Client
    {
        public long count_PAC_S_REC = 0;
        public long Last_Len_IMG { get; set; } = 0;
        public string URL { get; set; }
        public string MachineName { get; set; }
        public string UserName { get; set; }
        public bool JSON { get; set; } = true;
        public bool Tag_List_Changed { get; set; } = false;
        public bool ONS { get; set; } = true;
        public string attributes { get; set; }
        public string ResStr { get; set; }
        public DateTime LastRec { get; set; }
        public DateTime c_Date { get; set; }
        public DateTime Connected { get; set; }
        public bool Deny { get; set; }
        public bool IsActive { get; set; }
        public short Stat { get; set; }
        public Socket Socket { get; set; }
        public string TEXTBOX_BUFF { get; set; }
        public string account { get; set; }
        public int ID { get; set; } = -1;
        public string form_name { get; set; }
        public string IP { get; set; }
        public int timeout { get; set; } = 10;
        public long TotalSend { get; set; } = 0;
        public long TotalRecieve { get; set; } = 0;
        public string LastMSG { get; set; }
        public long In_Count { get; set; }
        public long Out_Count { get; set; }
        private Socket clientSocket;
        private bool ConnectedHost { get; set; } = false;

        public byte[] buffer { get; set; } = new byte[1024];
        public string TagsLst { get; set; } = "";
        public List<tag_lst> tags_lst { get; set; } = new List<tag_lst>();

        public struct tag_lst
        {
            public short num;
            public string value;
        }

        public short tags_lst_ADD(short index)
        {
            tag_lst tag = new tag_lst
            {
                num = index
            };
            this.tags_lst.Add(tag);
            return (short)(this.tags_lst.Count - 1);
        }

        public void tags_lst_WRITE(short index, string value)
        {
            tag_lst tag = this.tags_lst[index];
            tag.value = value;
            this.tags_lst[index] = tag;
            this.Tag_List_Changed = true;
        }

        public void SengJSON(short id)
        {
            string TEXT_BUFF = "";
            foreach (var tag in this.tags_lst)
            {
                if (!string.IsNullOrEmpty(TEXT_BUFF) && !string.IsNullOrEmpty(tag.value))
                    TEXT_BUFF += ",";
                TEXT_BUFF += tag.value;
                this.tags_lst_WRITE(tag.num, "");
            }

            if (TEXT_BUFF.Length != 0)
            {
                TEXT_BUFF = "[{\"time\":\"" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "\"," + TEXT_BUFF + "}]";
                this.SendAsync(TEXT_BUFF);
            }

            this.Tag_List_Changed = false;
        }

        public void Send_Image(short id)
        {
            if (!string.IsNullOrEmpty(this.TEXTBOX_BUFF))
            {
                SendAsync(this.TEXTBOX_BUFF);
                this.TEXTBOX_BUFF = "";
            }
        }

        private void OnReceive(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            int bytesRead;
            byte[] totalBytes;
            if (client.Connected)
            {
                try
                {
                    bytesRead = client.EndReceive(ar);
                    byte[] buffArray = new byte[client.Available];

                    if (client.Available > 0)
                    {
                        client.Receive(buffArray);
                        totalBytes = new byte[buffArray.Length + bytesRead];
                        Array.Copy(this.buffer, 0, totalBytes, 0, bytesRead);
                        Array.Copy(buffArray, 0, totalBytes, bytesRead, buffArray.Length);
                    }
                    else
                    {
                        byte[] totalBytes = new byte[bytesRead];
                        Array.Copy(this.buffer, 0, totalBytes, 0, bytesRead);
                    }

                    if (bytesRead > 1)
                    {
                        this.ResStr = this.CheckForDataAvailability(totalBytes, totalBytes.Length - 1);
                        count_PAC_S_REC++;
                        count_Byte_S_REC += totalBytes.Length - 1;

                        if (this.ResStr.ToLower().StartsWith("?form_name"))
                        {
                            Fill_client_delails(this.ID, this.ResStr);
                        }
                        else if (this.ResStr.ToLower().StartsWith("?image_bmp"))
                        {
                            this.JSON = false;
                            Send_Image(this.ID);
                        }

                        if (this.attributes == "brief")
                        {
                            if (Clients_LIST[this.ID].TagsLst.Length > 0)
                            {
                                var list = ParseArrString(Clients_LIST[this.ID].TagsLst, ',');
                                foreach (var num in list)
                                {
                                    TagsList[Convert.ToInt32(num.value)].subs_lsl_ADD(this.ID, Clients_LIST[this.ID].tags_lst_ADD(Convert.ToInt32(num.value)));
                                }
                            }
                        }

                        this.TotalRecieve += bytesRead;
                        this.LastMSG = this.ResStr.Substring(0, Math.Min(40, this.ResStr.Length));
                        this.In_Count += bytesRead;

                        if (this.ONS)
                        {
                            this.c_Date = DateTime.Now;
                        }

                        while (this.IsActive)
                        {
                            if (this.Tag_List_Changed && this.JSON)
                            {
                                SengJSON(this.ID);
                            }
                            else if (!this.JSON)
                            {
                                Send_Image(this.ID);
                            }

                            Thread.Sleep(this.timeout);
                        }

                        if (!string.IsNullOrWhiteSpace(JsonText))
                        {
                            // SendAsync(JsonText);
                        }

                        this.ProcessReceive();
                    }
                }
                catch (Exception ex)
                {
                    p_errlog("Sub OnRecieve", "BeginReceive", ex.Message);
                    this.IsActive = false;
                }
            }
            else
            {
                ProcessReceive();
            }
        }

        public void ProcessReceive()
        {
            try
            {
                if (Socket.Connected)
                {
                    Socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), Socket);
                }
            }
            catch (Exception ex)
            {
                SocketException(Socket, this.ID);
            }
        }

        public void SendAsync(string data)
        {
            try
            {
                if (this.IsActive)
                {
                    byte[] buff = WSMessage(data);
                    count_PAC_S_REC++;
                    count_Byte_S_REC += buff.Length - 1;
                    this.TotalSend += buff.Length;
                    this.Out_Count += buff.Length;

                    Socket.BeginSend(buff, 0, buff.Length, SocketFlags.None, new AsyncCallback(OnSend), Socket);
                }
            }
            catch (Exception ex)
            {
                SocketException(Socket, this.ID);
            }
        }

        private void OnSend(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            if (client.Connected)
            {
                try
                {
                    client.EndSend(ar);
                }
                catch (Exception)
                {
                    // Handle exception
                }
            }
        }

        public void StartConnection()
        {
            // Uncomment if you need to start a new thread
            // Thread t = new Thread(new ThreadStart(Main));
            // t.Start();
        }

        public Client(string p_URL, Socket inSocket)
        {
            URL = p_URL;
            Connected = DateTime.Now;
            Stat = 0;
            Socket = inSocket;
            IsActive = true;
            Deny = true;
            tags_lst = new List<tag_lst>();

            try
            {
                IsActive = true;
                Stat = 1;
                LastRec = DateTime.Now;
                Deny = false;
            }
            catch (Exception ex)
            {
                p_errlog("Sub_AddClient", "BeginReceive", ex.Message);
            }
        }

        private void Disconnect()
        {
            try
            {
                if (ConnectedHost)
                {
                    ConnectedHost = false;
                    int SBufferSize, RBufferSize;
                    bool NoDelay;
                    SBufferSize = clientSocket.SendBufferSize;
                    RBufferSize = clientSocket.ReceiveBufferSize;
                    NoDelay = clientSocket.NoDelay;
                    clientSocket.Disconnect(false);
                    clientSocket.Close();
                }
            }
            catch (Exception)
            {
                // Handle exception
            }
        }

        public string CheckForDataAvailability(byte[] inp_bytes, int leng)
        {
            int frameCount = 2;
            byte[] bytesArray = new byte[1];
            Array.Resize(ref bytesArray, leng);
            Array.Copy(inp_bytes, 0, bytesArray, 0, leng);

            if (bytesArray.Length > 1)
            {
                byte secondByte = bytesArray[1];
                uint theLength = (uint)(secondByte & 127);
                int indexFirstMask = 2;
                if (theLength == 126)
                {
                    indexFirstMask = 4;
                }
                else if (theLength == 127)
                {
                    indexFirstMask = 10;
                }
                List<byte> masks = new List<byte>();
                int x = indexFirstMask;
                while (x < indexFirstMask + 4)
                {
                    masks.Add(bytesArray[x]);
                    x++;
                }

                int indexFirstDataByte = indexFirstMask + 4;
                byte[] decoded = new byte[leng - indexFirstDataByte];
                int i = 0, j = 0;
                for (i = indexFirstDataByte; i < leng; i++, j++)
                {
                    byte mask = masks[j % 4];
                    byte encodedByte = bytesArray[i];
                    decoded[j] = (byte)(encodedByte ^ mask);
                }

                return Encoding.UTF8.GetString(decoded);
            }
            return "";
        }

        public byte[] WSMessage(string message)
        {
            byte opcode = 0x81;
            int payloadLength = message.Length;
            List<byte> frame = new List<byte>();
            frame.Add(opcode);

            if (payloadLength <= 125)
            {
                frame.Add((byte)payloadLength);
            }
            else if (payloadLength <= 0xFFFF)
            {
                frame.Add(0x7E);
                frame.Add((byte)((payloadLength >> 8) & 0xFF));
                frame.Add((byte)(payloadLength & 0xFF));
            }
            else
            {
                frame.Add(0x7F);
                frame.Add((byte)((payloadLength >> 56) & 0xFF));
                frame.Add((byte)((payloadLength >> 48) & 0xFF));
                frame.Add((byte)((payloadLength >> 40) & 0xFF));
                frame.Add((byte)((payloadLength >> 32) & 0xFF));
                frame.Add((byte)((payloadLength >> 24) & 0xFF));
                frame.Add((byte)((payloadLength >> 16) & 0xFF));
                frame.Add((byte)((payloadLength >> 8) & 0xFF));
                frame.Add((byte)(payloadLength & 0xFF));
            }

            frame.AddRange(Encoding.UTF8.GetBytes(message));
            return frame.ToArray();
        }

        public byte[] WSMessage_OLD(string message)
        {
            byte[] rawData = Encoding.UTF8.GetBytes(message);
            decimal len = rawData.Length;
            short frameCount = 0;
            byte[] frame = new byte[11];
            frame[0] = 129;

            if (rawData.Length <= 125)
            {
                frame[1] = (byte)rawData.Length;
                frameCount = 2;
            }
            else if (rawData.Length >= 126 && rawData.Length <= 65535)
            {
                frame[1] = 126;
                frame[2] = (byte)((len >> 8) & 255);
                frame[3] = (byte)(len & 255);
                frameCount = 4;
            }
            else
            {
                frame[1] = 127;
                frame[2] = (byte)((len >> 56) & 255);
                frame[3] = (byte)((len >> 48) & 255);
                frame[4] = (byte)((len >> 40) & 255);
                frame[5] = (byte)((len >> 32) & 255);
                frame[6] = (byte)((len >> 24) & 255);
                frame[7] = (byte)((len >> 16) & 255);
                frame[8] = (byte)((len >> 8) & 255);
                frame[9] = (byte)(len & 255);
                frameCount = 10;
            }

            int bLength = frameCount + rawData.Length;
            byte[] reply = new byte[bLength + 1];
            int bLim = 0;

            for (int i = 0; i < frameCount; i++)
            {
                reply[bLim] = frame[i];
                bLim++;
            }

            for (int i = 0; i < rawData.Length; i++)
            {
                reply[bLim] = rawData[i];
                bLim++;
            }

            return reply;
        }


    }
}