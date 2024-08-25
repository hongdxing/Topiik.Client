using System;
using System.Collections.Generic;
using System.Globalization;
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
            //var endpoint = IPEndPoint.Parse(addr);
            var endpoint = CreateIPEndPoint(addr);
            Socket client = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(endpoint);

            return client;
        }

        private static IPEndPoint CreateIPEndPoint(string endPoint)
        {
            string[] ep = endPoint.Split(':');
            if (ep.Length < 2) throw new FormatException("Invalid endpoint format");
            IPAddress? ipAddress = null;
            if (ep.Length > 2)
            {
                var hostOrIp = string.Join(":", ep, 0, ep.Length - 1);
                try
                {
                    ipAddress = IPAddress.Parse(hostOrIp);
                }
                catch { }
                if (ipAddress == null)
                {
                    IPHostEntry ipHostEntry = Dns.GetHostEntry(hostOrIp);
                    foreach (var addr in ipHostEntry.AddressList)
                    {
                        if (addr.AddressFamily == AddressFamily.InterNetworkV6)
                        {
                            ipAddress = addr;
                            break;
                        }
                    }
                }
            }
            else
            {
                try
                {
                    ipAddress = IPAddress.Parse(ep[0]);
                }
                catch { }
                if (ipAddress == null)
                {
                    IPHostEntry ipHostEntry = Dns.GetHostEntry(ep[0]);
                    foreach (var addr in ipHostEntry.AddressList)
                    {
                        if (addr.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipAddress = addr;
                            break;
                        }
                    }
                }
            }
            if (ipAddress != null)
            {
                int port;
                if (!int.TryParse(ep[ep.Length - 1], NumberStyles.None, NumberFormatInfo.CurrentInfo, out port))
                {
                    throw new FormatException("Invalid port");
                }
                return new IPEndPoint(ipAddress, port);
            }
            throw new Exception("Invalid ip");
        }
    }
}

