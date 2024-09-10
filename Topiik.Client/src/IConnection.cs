using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topiik.Client
{
    public interface IConnection
    {
        public bool InUse { get; set; }     

        dynamic? Execute(byte[]data);

        void Close();
    }
}
