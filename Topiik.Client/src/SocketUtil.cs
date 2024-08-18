using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Topiik.Client
{
    internal class SocketUtil
    {
        public static Socket PrepareSocketClient(string addr)
        {
            var endpoint = IPEndPoint.Parse(addr);
            Socket client = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(endpoint);

            return client;
        }
    }
}

