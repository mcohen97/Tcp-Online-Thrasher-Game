﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public class ConnectionLostException:Exception
    {
        public ConnectionLostException(string msg) : base(msg)
        {
        }

    }
}
