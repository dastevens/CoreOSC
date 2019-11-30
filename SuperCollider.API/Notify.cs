using CoreOSC;
using System;
using System.Threading.Tasks;

namespace SuperCollider.API
{
    public class Notify : IRequest<Done>
    {
        public OscMessage Message => new OscMessage(new Address("/notify"));

        public Done Response { get; } = new Done();
    }
}
