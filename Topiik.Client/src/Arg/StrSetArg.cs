using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topiik.Clien.Arg
{
    public class StrSetArg
    {
        /*
         * set time to live of this key
         * default value is int64 max, means live forever
         * 
         */
        public long TTL { get; set; } = long.MaxValue;

        /*
         * if true then return value before set
         * 
         */
        public bool GET { get; set; } = false;

        /*
         * Determine argument EX|NX 
         * if null then both EX and NX not included
         * if true then argument EX included
         * if false then argument NX included
         */
        public Nullable<bool> EX { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if(TTL > 0)
            {
                sb.Append($"TTL {TTL} ");
            }
            if (GET)
            {
                sb.Append("GET ");
            }
            if (EX != null)
            {
                if (EX.Value)
                {
                    sb.Append("EX");
                }
                else
                {
                    sb.Append("NX");
                }
            }
            return sb.ToString().Trim();
        }
    }
}
