
using System.Configuration;
using System.Net.Sockets;
using System.Text;
using Topiik.Client.Arg;
using Topiik.Client.Interface;

namespace Topiik.Client
{
    public class TopiikClient : ITopiikClient
    {
        IConnectionFactory connFactory;
        IConnection connection;

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

        #region IKeyCommand
        public long Del(params string[] keys)
        {
            var data = Req.Build(Commands.DEL).WithKeys(keys).Marshal();
            connection.Send(data);
            return connection.Receive();
        }

        public bool Exists(string key)
        {
            var data = Req.Build(Commands.EXISTS).WithKey(key).Marshal();
            var result = connection.Execute(data);
            if (result.Count > 0)
            {
                return result[0] == "T" ? true : false;
            }
            return false;
        }

        public IEnumerable<bool> Exists(params string[] keys)
        {
            var data = Req.Build(Commands.EXISTS).WithKeys(keys).Marshal();
            var result = connection.Execute(data);
            foreach (var str in result)
            {
                yield return str == "T" ? true : false;
            }
        }

        /*
         * Get ttl of key
         * Return:
         *  - ttl if key exists and ttl greater than 0
         *  - -1 if ttl not set
         *  - -2 if key not exists
         */
        public long Ttl(string key)
        {
            var data = Req.Build(Commands.TTL).WithKey(key).Marshal();
            connection.Send(data);
            return connection.Receive();
        }

        /*
         * Set ttl of key
         * Return:
         *  - 1 if the ttl set
         *  - -2 if the key not exists
         */
        public long Ttl(string key, long seconds)
        {
            var args = new TtlArg { Seconds = seconds };
            var data = Req.Build(Commands.TTL).WithKey(key).WithArgs(args).Marshal();
            connection.Send(data);
            return connection.Receive();
        }

        public long Ttl(string key, TtlArg args)
        {
            var data = Req.Build(Commands.DEL).WithKey(key).WithArgs(args).Marshal();
            connection.Send(data);
            return connection.Receive();
        }

        #endregion

        #region IStringCommand
        public string Get(string key)
        {
            var data = Req.Build(Commands.GET).WithKey(key).Marshal();
            connection.Send(data);
            var result = connection.Receive();
            return result;
        }

        public List<string> GetM(params string[] keys)
        {
            var data = Req.Build(Commands.GETM).WithKeys(keys).Marshal();
            connection.Send(data);
            return connection.Receive();
        }

        public long Incr(string key, long step = 1)
        {
            var data = Req.Build(Commands.INCR).WithKey(key).Marshal();
            connection.Send(data);
            return connection.Receive();
        }

        public string Set(string key, string value, StrSetArg? args)
        {
            /* build data */
            var data = Req.Build(Commands.SET).WithKey(key).WithVal(value).WithArgs(args).Marshal();

            /* send */
            connection.Send(data);

            /* get result */
            var result = connection.Receive();

            return result;
        }

        public string SetM(Dictionary<string, string> keyValues)
        {
            var data = Req.Build(Commands.SETM).WithKeys(keyValues.Keys.ToArray())
                .WithVals(keyValues.Values.ToArray()).Marshal();
            connection.Send(data);
            return connection.Receive();
        }
        #endregion

        #region IListCommand
        public long LPush(string key, params string[] values)
        {
            var data = Req.Build(Commands.LPUSH).WithKey(key).WithVals(values).Marshal();
            connection.Send(data);
            return connection.Receive();
        }

        public long LPushR(string key, params string[] values)
        {
            var data = Req.Build(Commands.LPUSHR).WithKey(key).WithVals(values).Marshal();
            connection.Send(data);
            return connection.Receive();
        }

        public List<string> LPop(string key, int count = 1)
        {
            var args = new ListPopArg { Count = count };
            var data = Req.Build(Commands.LPOP).WithKey(key).WithArgs(args).Marshal();
            connection.Send(data);
            return connection.Receive();
        }

        public List<string> LPopR(string key, int count = 1)
        {
            var args = new ListPopArg { Count = count };
            var data = Req.Build(Commands.LPOPR).WithKey(key).WithArgs(args).Marshal();
            connection.Send(data);
            return connection.Receive();
        }

        public long LLen(string key)
        {
            var data = Req.Build(Commands.LLEN).WithKey(key).Marshal();
            connection.Send(data);
            return connection.Receive();
        }

        public List<string> LRange(string key, int start, int end)
        {
            var args = new ListRangeArg { Start = start, End = end };
            var data = Req.Build(Commands.LRANGE).WithKey(key).WithArgs(args).Marshal();
            connection.Send(data);
            return connection.Receive();
        }

        public string LSet(string key, string value, int index)
        {
            var data = Req.Build(Commands.LSET).WithKey(key).WithVal(value).WithArgs(index).Marshal();
            return connection.Execute(data);
        }
        #endregion

        public void Dispose()
        {
            Close();
        }

        public void Close()
        {
            try
            {
                if (connection != null)
                {
                    if (connection.InUse)
                    {
                        connection.InUse = false;
                    }
                    else
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }
}