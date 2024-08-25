using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Topiik.Client.src;

namespace Topiik.Client
{
    public class Connection: IConnection
    {
        public Socket socket { get; private set; }
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

        public int Send(byte[] data)
        {
            return socket.Send(data);
        }

        #region private
        string GetControllerLeaderAddr(string addr)
        {
            using (var client = SocketUtil.PrepareSocketClient(addr))
            {
                var buf = Proto.EncodeHeader(Commands.GET_CTLADDR, 1);
                var req = new Req();
                var data = req.Marshal();

                // merge header and data(req)
                buf = buf.Concat(data).ToArray();

                // send
                client.Send(buf);

                BinaryReader br = new BinaryReader(new MemoryStream(buf));

                var header = ReceiveHeader(client);
                var result = ReceiveBody(client, header.len);

                return BitConverter.ToString(result);
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

        public dynamic Receive()
        {
            var header = ReceiveHeader(socket);
            var body = ReceiveBody(socket, header.len);
            if (header.flag == 0)
            {
                throw new Exception("TODO");
            }
            if (header.datatye == Response.ResString)
            {
                var rslt = Encoding.UTF8.GetString(body);
                return rslt;
            }
            else if (header.datatye == Response.ResStringArray)
            {
                return new List<string>();
            }
            else if (header.datatye == Response.ResIneger)
            {
                BitConverter.ToInt64(body);
                return 0;
            }
            else
            {
                return "";
            }

        }
        #endregion
    }
}
