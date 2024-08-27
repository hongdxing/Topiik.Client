using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topiik.Client.src.Arg
{
    public class ListRangeArg
    {
        public long Start { get; set; }
        public long End { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"{Start} {End}");
            return sb.ToString();
        }
    }
}
