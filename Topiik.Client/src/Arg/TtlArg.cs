using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topiik.Client.Arg
{
    public class TtlArg
    {
        public long Seconds { get; set; }
        public bool At { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"{Seconds} ");

            if (At)
            {
                sb.Append("AT");
            }

            return sb.ToString().Trim();
        }
    }
}
