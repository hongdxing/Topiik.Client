using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topiik.Client.Arg
{
    public class ListPopArg
    {
        /*
         * how many values pop from list
         */
        public int Count { get; set; }

        public override string ToString()
        {
            if (Count == 0) return string.Empty;
            var sb = new StringBuilder();
            sb.Append($"{Count}");
            return sb.ToString();
        }
    }
}
