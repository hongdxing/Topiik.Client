﻿using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Topiik.Client
{
    public class Connection : IConnection
    {
        private Socket socket;

        public bool InUse { get; set; }

        public Connection(List<string> ctlAddrs)
        {
            foreach (var addr in ctlAddrs)
            {
                try
                {
                    var leaderAddr = GetControllerLeaderAddr(addr);
                    socket = SocketUtil.PrepareSocketClient(leaderAddr);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
            }

        }

        #region private
        string GetControllerLeaderAddr(string addr)
        {
            using (var client = SocketUtil.PrepareSocketClient(addr))
            {
                var data = Req.Build(Commands.GET_CTLADDR).Marshal();

                client.Send(data);

                var header = ReceiveHeader(client);
                /* -2: 1 byte for flag, 1 byte for result type */
                var result = ReceiveBody(client, header.len - 2);

                return Encoding.UTF8.GetString(result);
            }
        }

        private (int len, byte flag, byte datatye) ReceiveHeader(Socket client)
        {
            (int len, byte flag, byte datatye) header = (0, 0, 0);
            var buf = RecevieFixLenBytes(client, Proto.PROTO_HEADER_LEN);
            header = Proto.ParseHeader(buf);

            return header;
        }

        private byte[] ReceiveBody(Socket client, int len)
        {
            var buf = RecevieFixLenBytes(client, len);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(buf);
            }
            return buf;
        }

        private byte[] RecevieFixLenBytes(Socket client, int toRead)
        {
            var bytes = new byte[toRead];
            int read = client.Receive(bytes);

            while (read < toRead)
            {
                read += client.Receive(bytes, read, toRead - read, SocketFlags.None);
            }

            return bytes;
        }

        public dynamic? Execute(byte[] data)
        {
            (int len, byte flag, byte datatye) header = (0, 0, 0);
            byte[] body = { };

            socket.Send(data);
            header = ReceiveHeader(socket);
            body = ReceiveBody(socket, header.len - 2);

            if (header.flag == 0)
            {
                throw new Exception(Encoding.UTF8.GetString(body));
            }
            if (header.datatye == Response.ResString)
            {
                var val = Encoding.UTF8.GetString(body);
                if (val == Consts.Nil)
                {
                    return null;
                }
                return val;
            }
            else if (header.datatye == Response.ResStringArray)
            {
                var vals = JsonSerializer.Deserialize<List<String?>>(body);
                if (vals == null)
                {
                    return new List<string>();
                }
                for (var i = 0; i < vals.Count; i++)
                {
                    if (vals[i] == Consts.Nil)
                    {
                        vals[i] = null;
                    }
                }
                return vals;
            }
            else if (header.datatye == Response.ResIneger)
            {
                var rslt = BitConverter.ToInt64(body);
                return rslt;
            }
            else if (header.datatye == Response.ResIntegerArray)
            {
                var rslt = JsonSerializer.Deserialize<List<long>>(body);
                return rslt == null ? new List<long>() : rslt;
            }
            else
            {
                return "";
            }
        }

        public void Close()
        {
            if (socket != null)
            {
                socket.Close();
            }
        }

        #endregion
    }
}
