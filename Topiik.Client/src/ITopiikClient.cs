using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Topiik.Client
{
    public interface ITopiikClient
    {

        void Connect();

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

        /* String Begin */
        string Set(string key, string value, long ttl = long.MaxValue, bool get= false, Nullable<Boolean> exist = null);
        string Get(string key);
        string SetM(List<string> keys, List<string> values);
        List<string> GetM(List<String> keys);


        /* List Begin */

        /*
         * Push value to list header(Left)
         * Parameters:
         *  - key: the key of list
         *  - value: the value
         * Return:
         *  - lenght of list after push
         */
        long LPush(string key, string value);

        /*
         * Push value to list tail(Right)
         * Parameters:
         *  - key: the key of list
         *  - value: the value
         * Return:
         *  - length of list after push
         */
        long LPushR(string key, string value);

        /*
         * Pop value(s) from header(Left)
         * Parameters:
         *  - key: the key of list
         *  - count: how many values to pop from list
         * Return:
         *  - list of values
         */
        List<string> LPop(string key, int count);

        /* Pop value(s) from list tail(Right)
         * Parameters:
         *  - key: the key of list
         *  - count: how many values to pop from list
         * Return:
         *  - list of values
         */
        List<string> LPopR(string key, int count);

        /*
         * Get length of list
         * Parameters:
         *  - key: the key of list
         * Return:
         *  - length of list
         */
        long Llen(string key);
    }
}
