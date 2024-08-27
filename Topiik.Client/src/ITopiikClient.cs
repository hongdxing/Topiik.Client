using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Topiik.Client.Interface;

namespace Topiik.Client
{
    public interface ITopiikClient : IKeyCommand, IListCommand, IStringCommand
    {


    }
}
