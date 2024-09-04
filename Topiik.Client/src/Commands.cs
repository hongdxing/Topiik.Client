using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topiik.Client
{
    internal class Commands
    {

        public const byte GET_CTLADDR = 15;

        /* String */
        public const byte SET = 16;
        public const byte GET = 17;
        public const byte SETM = 18;
        public const byte GETM = 19;
        public const byte INCR = 20;

        /* List */
        public const byte LPUSH = 32;
        public const byte LPOP = 33;
        public const byte LPUSHR = 34;
        public const byte LPOPR = 35;
        public const byte LLEN = 36;
        public const byte LRANGE = 37;
        public const byte LSET = 38;

        /* Key */
        public const byte TTL = 176;
        public const byte DEL = 177;
        public const byte KEYS = 178;
        public const byte EXISTS = 179;
    }
}
