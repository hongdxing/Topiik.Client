using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topiik.Client.Arg;

namespace Topiik.Client.Interface
{
    public interface IStringCommand
    {
        string Set(string key, string value, StrSetArg? args = null);
        string Get(string key);
        string SetM(Dictionary<string, string> keyValues);
        List<string> GetM(params string[] keys);
        long Incr(string key, long step = 1);
    }
}
