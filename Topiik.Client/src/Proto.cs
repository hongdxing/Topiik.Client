using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topiik.Client
{
    internal class Proto
    {
        /*
         * Server reponse proto format
         * ------------Reposne Lenght(4)---------Flag(1)---Datatype(1)----
         * 00000000 00000000 00000000 00000000 | 00000000 | 00000000 | ...
         * ---------------------------------------------------------------
         * Flag:
         *  - 0: fail
         *  - 1: success
         * Datatype:
         *  - 1: string
         *  - 2: integer
         *  - 3: string array
         */
        public const int PROTO_HEADER_LEN = 6; // response len(4) + response flag(1) + response datatype(1)

        public static (int len, byte flag, byte datatype) ParseHeader(byte[] buf)
        {
            (int len, byte flag, byte datatype) header=(0,0,0);
            if (buf == null || buf.Length < PROTO_HEADER_LEN)
            {
                return header;
            }
            /* 
             * server side is Little Endian
             * so if client side is Big Endian, reverse reposne first
            */
            var lenBytes = buf.Take(4).ToArray();
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(lenBytes);
                header.len= BitConverter.ToInt32(lenBytes, 0);
            }
            else
            {
                header.len = BitConverter.ToInt32(lenBytes, 0);
            }
            header.flag = buf[4];
            header.datatype= buf[5];
            return header;
        }

        public static byte[] EncodeHeader(byte icmd, byte ver=1)
        {
            byte[] buf = {ver, icmd };

            return buf;
        }

        public static byte[] Encode(byte[] data)
        {
            // length
            var len = Convert.ToInt32(data.Length);
            byte[] buf = BitConverter.GetBytes(len);

            /*
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(buf);
            }
            */

            return buf.Concat(data).ToArray();
        }

        public static void Decode()
        {

        }
    }
}
