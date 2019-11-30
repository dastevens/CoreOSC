using CoreOSC;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SuperCollider.API
{
    public class DLoad
    {
        public DLoad(string pathname)
        {
            Pathname = pathname;
        }

        public OscMessage Message => new OscMessage(new Address("/d_load"), new object[] { Pathname });

        public string Pathname { get; }
    }
}
