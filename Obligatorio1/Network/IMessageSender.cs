﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public interface IMessageSender
    {
        void SendMessage(string message);
    }
}
