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
        public int MaxPoolSzie { get; private set; }
        public int IdlePoolSize { get; private set; }

        public ConnectionFactory(string servers, int maxPoolSzie=15, int idlePoolSize=5)
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
            Connection? conn;
            lock (pool)
            {
                conn = pool.Where(x => !x.InUse).FirstOrDefault();
                if (conn == null)
                {
                    conn = new Connection(ServerList);
                    if (pool.Count < MaxPoolSzie)
                    {
                        Console.WriteLine($"Pool size {pool.Count}");
                        conn.InUse = true;
                        pool.Add(conn);
                    }
                }
                else
                {
                    Console.WriteLine("Get connection from pool");
                    conn.InUse = true;
                }
            }
            return conn;
        }
    }
}
