using CoreOSC;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SuperCollider.API
{
    public class Version : IRequest<VersionReply>
    {
        public OscMessage Message => new OscMessage(new Address("/version"));
    }
}
