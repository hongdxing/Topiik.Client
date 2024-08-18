using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Topiik.Client
{
    internal class Req
    {
        public List<string> Keys { get; set; } = [];
        public List<string> Vals { get; set; } = [];
        public string Args { get; set; } = string.Empty;

        public byte[] Marshal()
        {
            var json = JsonSerializer.Serialize(this);
            var buf = Encoding.UTF8.GetBytes(json);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(buf);
            }
            return buf;
        }
    }
}
