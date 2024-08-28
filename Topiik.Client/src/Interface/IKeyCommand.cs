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
        bool Exists(string key);
        long Exists(params string[] keys);

        /*
         * Get ttl of key
         * Return:
         *  - ttl if key exists and ttl greater than 0
         *  - -1 if ttl not set
         *  - -2 if key not exists
         */
        long Ttl(string key);

        /*
         * Set ttl of key
         * Return:
         *  - 1 if the ttl set
         *  - -2 if the key not exists
         */
        long Ttl(string key, long seconds);
        //long Ttl(string key, TtlArg args);
    }
}
