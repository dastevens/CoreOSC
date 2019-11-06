namespace CoreOSC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class OscBundle : OscPacket
    {
        private Timetag timetag;

        public OscBundle(ulong timetag, params OscMessage[] args)
        {
            this.timetag = new Timetag(timetag);
            this.Messages.AddRange(args);
        }

        public DateTime Timestamp
        {
            get { return this.timetag.Timestamp; }
        }

        public List<OscMessage> Messages { get; } = new List<OscMessage>();

        public override byte[] GetBytes()
        {
            var bundle = "#bundle";
            var bundleTagLen = Utils.AlignedStringLength(bundle);
            var tag = SetULong(this.timetag.Tag);

            var outMessages = new List<byte[]>();
            foreach (var msg in this.Messages)
            {
                outMessages.Add(msg.GetBytes());
            }

            var len = bundleTagLen + tag.Length + outMessages.Sum(x => x.Length + 4);

            var i = 0;
            var output = new byte[len];
            Encoding.ASCII.GetBytes(bundle).CopyTo(output, i);
            i += bundleTagLen;
            tag.CopyTo(output, i);
            i += tag.Length;

            foreach (var msg in outMessages)
            {
                var size = SetInt(msg.Length);
                size.CopyTo(output, i);
                i += size.Length;

                msg.CopyTo(output, i);
                i += msg.Length; // msg size is always a multiple of 4
            }

            return output;
        }
    }
}