using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Markup;
using Topiik.Clien.Arg;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Topiik.Client
{
    internal class Req
    {
        private byte[] m_header;
        public List<byte[]> Keys { get; set; } = [];
        public List<byte[]> Vals { get; set; } = [];
        public string Args { get; set; } = string.Empty;

        private Req(byte[] header)
        {
            m_header = header;
        }

        public static Req Build(byte command, byte ver = 1)
        {
            var header = Proto.EncodeHeader(command, ver);
            return new Req(header);
        }

        public Req WithKey(string key)
        {
            this.Keys.Add(Encoding.UTF8.GetBytes(key));
            return this;
        }

        public Req WithVal(string val)
        {
            this.Vals.Add(Encoding.UTF8.GetBytes(val));
            return this;
        }

        public Req WithArgs(object args)
        {
            if (args == null)
            {
                return this;
            }
            string? sArg = string.Empty;
            if (args.GetType() == typeof(StrSetArg))
            {
                sArg = args.ToString();
            }
            this.Args = sArg == null ? string.Empty : sArg;
            return this;
        }

        public byte[] Marshal()
        {
            var json = JsonSerializer.Serialize(this);
            Console.WriteLine(json);
            var buf = Encoding.UTF8.GetBytes(json);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(buf);
            }
            return Proto.Encode(m_header.Concat(buf).ToArray());
        }
    }
}
