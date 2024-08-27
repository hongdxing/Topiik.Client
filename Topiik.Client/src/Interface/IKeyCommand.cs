using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topiik.Client.Arg;

namespace Topiik.Client.Interface
{
    public interface IKeyCommand
    {
        long Del(params string[] keys);
        long Exists(params string[] keys);
        long Ttl(string key, long seconds);
        long Ttl(string key, TtlArg args);
    }
}
