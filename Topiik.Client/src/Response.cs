using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topiik.Client
{
    internal class Response
    {
        public const byte ResString = 1;
        public const byte ResStringArray = 2;
        public const byte ResIneger = 3;
        public const byte ResIntegerArray = 4;
        public const byte ResDouble = 5;
        public const byte ResDoubleArray = 6;
        public const byte ResByte = 7;
        public const byte ResByteArray = 8;
        public const byte ResMap = 9;
        public const byte ResSet = 10;
    }
}
