using CoreOSC;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SuperCollider.API
{
    public class DLoadDir
    {
        public DLoadDir(string pathname)
        {
            Pathname = pathname;
        }

        public OscMessage Message => new OscMessage(new Address("/d_loadDir"), new object[] { Pathname });

        public string Pathname { get; }
    }
}
