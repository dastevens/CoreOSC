﻿using CoreOSC;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SuperCollider.API
{
    public class Status
    {
        public OscMessage Message => new OscMessage(new Address("/status"));
    }
}