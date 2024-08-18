
using System.Configuration;
using System.Net.Sockets;

namespace Topiik.Client
{
    public class TopiikClient : ITopiikClient
    {

        List<string> ServerList { get; set; } = [];
       

        Socket socket;
        public TopiikClient(string servers)
        {
            ServerList.AddRange(servers.Split(','));
            if (ServerList.Count == 0)
            {
                throw new Exception("servers cannot be empty");
            }
        }

        public void Connect()
        {
            var controllerLeaderAddr = GetControllerLeaderAddr();
            System.Console.WriteLine(controllerLeaderAddr);
        }

        private string GetControllerLeaderAddr()
        {
            using (var client = SocketUtil.PrepareSocketClient(ServerList[0]))
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
                var result = ReceiveData(client, header.len);

                return BitConverter.ToString(result);
            }
        }

        private (int len, byte flag, byte datatye) ReceiveHeader(Socket client)
        {
            (int len, byte flag, byte datatye) header = (0, 0, 0);
            byte[] buf = new byte[Proto.PROTO_HEADER_LEN];
            var len = client.Receive(buf);
            if (len != Proto.PROTO_HEADER_LEN)
            {
                throw new Exception("Receive header failed");
            }
            header = Proto.ParseHeader(buf);

            return header;
        }

        private byte[] ReceiveData(Socket client, int len)
        {
            byte[] buf = new byte[len];
            var receivedLen = client.Receive(buf);
            if (receivedLen != len)
            {
                throw new Exception("Receive data failed");
            }
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(buf);
            }
            return buf;
        }

        /*
        private void Receive(Socket socket)
        {
            int receivedLen = 0;
            byte[] staticBuf = new byte[1024];
            byte[] dynamicBuf = new byte[] { };

            do
            {
                receivedLen = socket.Receive(staticBuf);
                dynamicBuf = combineBytes(dynamicBuf, 0, dynamicBuf.Length, staticBuf, 0, staticBuf.Length);
                if (receivedLen <= 0)
                {
                    break;
                }
                else if (dynamicBuf.Length < Proto.PROTO_HEADER_LEN)
                {
                    continue;
                }
                else
                {
                    var header = Proto.ParseHeader(dynamicBuf);
                    while(dynamicBuf.Length- Proto.PROTO_HEADER_LEN>= header.len)
                    {

                    }
                }

            } while (receivedLen > 0);
        }

        private byte[] combineBytes(byte[] firstBytes, int firstIndex, int firstLength, byte[] secondBytes, int secondIndex, int secondLength)
        {
            byte[] bytes;
            MemoryStream ms = new MemoryStream();
            ms.Write(firstBytes, firstIndex, firstLength);
            ms.Write(secondBytes, secondIndex, secondLength);
            bytes = ms.ToArray();
            ms.Close();
            return (bytes);
        }
        */


        /* Command section Begin */
        public string Get(string key)
        {
            var req = new Req { Keys = { key } };
            var data = req.Marshal();
            var msg = Proto.Encode(data);
            socket.Send(data);


            return string.Empty;
        }

        public List<string> GetM(List<string> keys)
        {
            throw new NotImplementedException();
        }

        public string Set(string key, string value, long ttl = long.MaxValue, bool get = false, bool? exist = null)
        {
            throw new NotImplementedException();
        }

        public string SetM(List<string> keys, List<string> values)
        {
            throw new NotImplementedException();
        }

        public long LPush(string key, string value)
        {
            throw new NotImplementedException();
        }

        public long LPushR(string key, string value)
        {
            throw new NotImplementedException();
        }

        public List<string> LPop(string key, int count)
        {
            throw new NotImplementedException();
        }

        public List<string> LPopR(string key, int count)
        {
            throw new NotImplementedException();
        }

        public long Llen(string key)
        {
            throw new NotImplementedException();
        }
    }
}
