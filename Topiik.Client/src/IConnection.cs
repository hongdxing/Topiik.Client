﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topiik.Client
{
    public interface IConnection
    {
        int Send(byte[] data);

        dynamic Execute(byte[]data);
    }
}
