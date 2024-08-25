﻿
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
            var data = Req.Build(Commands.GET).WithKey(key).Marshal();
            connection.Send(data);
            var result = connection.Receive();
            return result;
        }

        public List<string> GetM(List<string> keys)
        {
            var data = Req.Build(Commands.GETM).WithKeys(keys).Marshal();
            connection.Send(data);
            return connection.Receive();
        }

        public string Set(string key, string value, StrSetArg args)
        {
            /* build data */
            var data = Req.Build(Commands.SET).WithKey(key).WithVal(value).WithArgs(args).Marshal();

            /* send */
            connection.Send(data);

            /* get result */
            var result = connection.Receive();

            return result;
        }

        public string SetM(List<string> keys, List<string> values)
        {
            var data = Req.Build(Commands.SETM).WithKeys(keys).WithVals(values).Marshal();
            connection.Send(data);
            return connection.Receive();
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
