
using System.Configuration;
using System.Net.Sockets;
using System.Text;
using Topiik.Clien.Arg;
using Topiik.Client.Interface;
using Topiik.Client.src;

namespace Topiik.Client
{
    public class TopiikClient : ITopiikClient, IStringCommand, IListCommand
    {
        IConnectionFactory connFactory;
        Connection connection;

        public TopiikClient(IConnectionFactory connectionFactory)
        {
            connFactory = connectionFactory;
            connection = connectionFactory.GetConnection();
        }

        #region private

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
                    while (dynamicBuf.Length - Proto.PROTO_HEADER_LEN >= header.len)
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

        #endregion

        #region String
        public string Get(string key)
        {
            /* header */
            var buf = Proto.EncodeHeader(Commands.SET);

            /* req */
            var req = new Req { Keys = { Encoding.UTF8.GetBytes(key) } };
            var data = req.Marshal();

            /* header + req */
            buf = buf.Concat(data).ToArray();

            /* encode */
            buf = Proto.Encode(data);
            connection.Send(buf);

            var result = connection.Receive();

            return string.Empty;
        }

        public List<string> GetM(List<string> keys)
        {
            throw new NotImplementedException();
        }

        public string Set(string key, string value, StrSetArg args)
        {
            throw new NotImplementedException();
        }

        public string SetM(List<string> keys, List<string> values)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region List
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
        #endregion
    }
}
