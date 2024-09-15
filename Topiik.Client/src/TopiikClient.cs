
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

        #region Private

        private dynamic? Execute(byte[] data)
        {
            long time1 = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            for (; ; )
            {
                try
                {
                    return connection.Execute(data);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.Message);
                    // 5 seconds, TODO: configurable?
                    if (DateTimeOffset.Now.ToUnixTimeMilliseconds() - time1 < 5000)
                    {
                        // refresh connection
                        if (connection != null) { connection.Close(); }
                        connection = connFactory.GetConnection();
                        continue;
                    }
                }
            }
        }

        #endregion

        #region IKeyCommand
        public long Del(params string[] keys)
        {
            var data = Req.Build(Commands.DEL).WithKeys(keys).Marshal();
            return Execute(data);
        }

        public bool Exists(string key)
        {
            var data = Req.Build(Commands.EXISTS).WithKey(key).Marshal();
            var result = Execute(data);
            if (result.Count > 0)
            {
                return result[0] == "T" ? true : false;
            }
            return false;
        }

        public IEnumerable<bool> Exists(params string[] keys)
        {
            var data = Req.Build(Commands.EXISTS).WithKeys(keys).Marshal();
            var result = Execute(data);
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
            return connection.Execute(data);
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
            return Execute(data);
        }

        public long Ttl(string key, TtlArg args)
        {
            var data = Req.Build(Commands.DEL).WithKey(key).WithArgs(args).Marshal();
            return Execute(data);
        }

        #endregion

        #region IStringCommand
        public string Get(string key)
        {
            var data = Req.Build(Commands.GET).WithKey(key).Marshal();
            return Execute(data);
        }

        public List<string> GetM(params string[] keys)
        {
            var data = Req.Build(Commands.GETM).WithKeys(keys).Marshal();
            var vals = Execute(data);
            return vals == null ? new List<string>() : vals;
        }

        public long Incr(string key, long step = 1)
        {
            var data = Req.Build(Commands.INCR).WithKey(key).Marshal();
            return Execute(data);
        }

        public string Set(string key, string value, StrSetArg? args)
        {
            var data = Req.Build(Commands.SET).WithKey(key).WithVal(value).WithArgs(args).Marshal();
            return Execute(data);
        }

        public string SetM(Dictionary<string, string> keyValues)
        {
            var data = Req.Build(Commands.SETM).WithKeys(keyValues.Keys.ToArray())
                .WithVals(keyValues.Values.ToArray()).Marshal();
            return Execute(data);
        }
        #endregion

        #region IListCommand
        public long LPush(string key, params string[] values)
        {
            var data = Req.Build(Commands.LPUSH).WithKey(key).WithVals(values).Marshal();
            return Execute(data);
        }

        public long LPushR(string key, params string[] values)
        {
            var data = Req.Build(Commands.LPUSHR).WithKey(key).WithVals(values).Marshal();
            return Execute(data);
        }

        public List<string> LPop(string key, int count = 1)
        {
            var args = new ListPopArg { Count = count };
            var data = Req.Build(Commands.LPOP).WithKey(key).WithArgs(args).Marshal();
            var vals = Execute(data);
            return vals == null ? new List<string>() : vals;
        }

        public List<string> LPopR(string key, int count = 1)
        {
            var args = new ListPopArg { Count = count };
            var data = Req.Build(Commands.LPOPR).WithKey(key).WithArgs(args).Marshal();
            var vals = Execute(data);
            return vals == null ? new List<string>() : vals;
        }

        public long LLen(string key)
        {
            var data = Req.Build(Commands.LLEN).WithKey(key).Marshal();
            return Execute(data);
        }

        public List<string> LSlice(string key, int start, int end)
        {
            var args = new ListSliceArg { Start = start, End = end };
            var data = Req.Build(Commands.LRANGE).WithKey(key).WithArgs(args).Marshal();
            var vals = Execute(data);
            return vals == null ? new List<string>() : vals;
        }

        public string LSet(string key, string value, int index)
        {
            var data = Req.Build(Commands.LSET).WithKey(key).WithVal(value).WithArgs(index).Marshal();
            return Execute(data);
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