using CoreOSC;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperCollider.API
{
    public interface IRequest<T>
    {
        OscMessage Message { get; }
    }
}
