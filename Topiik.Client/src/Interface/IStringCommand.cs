using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topiik.Clien.Arg;

namespace Topiik.Client.Interface
{
    public interface IStringCommand
    {
        string Set(string key, string value, StrSetArg? args=null);
        string Get(string key);
        string SetM(List<string> keys, List<string> values);
        List<string> GetM(List<String> keys);
    }
}
