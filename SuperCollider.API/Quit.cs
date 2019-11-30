using CoreOSC;
using System;
using System.Threading.Tasks;

namespace SuperCollider.API
{
    public class Quit
    {
        public OscMessage Message => new OscMessage(new Address("/quit"));
    }
}
