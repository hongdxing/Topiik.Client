using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topiik.Client
{
    public class ConnectionFactory : IConnectionFactory
    {
        private List<Connection> pool = new List<Connection>();

        public List<string> ServerList { get; private set; } = [];
        public int MaxPoolSzie { get; private set; } = 50;
        public int IdlePoolSize { get; private set; } = 5;

        public ConnectionFactory(string servers, int maxPoolSzie=50, int idlePoolSize=5)
        {
            MaxPoolSzie = maxPoolSzie;
            IdlePoolSize = idlePoolSize;
            ServerList.AddRange(servers.Split(','));
            if (ServerList.Count == 0)
            {
                throw new Exception("servers cannot be empty");
            }
        }

        public Connection GetConnection()
        {
            Connection conn;
            lock (pool)
            {
                if (pool.Count > 0)
                {
                    conn = pool.First();
                }
                else
                {
                    conn = new Connection(ServerList);
                    if (pool.Count < MaxPoolSzie)
                    {
                        pool.Add(conn);
                    }
                }
            }
            return conn;
        }
    }
}
