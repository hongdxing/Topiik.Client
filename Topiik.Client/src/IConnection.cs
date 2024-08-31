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
        int Send(byte[] data);

        public dynamic Receive();

        dynamic Execute(byte[]data);

        void Close();
    }
}
