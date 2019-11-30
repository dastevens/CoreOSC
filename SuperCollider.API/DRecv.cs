using CoreOSC;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SuperCollider.API
{
    public class DRecv
    {
        public DRecv(byte[] bufferOfData)
        {
            BufferOfData = bufferOfData;
        }

        public OscMessage Message => new OscMessage(new Address("/d_recv"), new object[] { BufferOfData });

        public byte[] BufferOfData { get; }
    }
}
