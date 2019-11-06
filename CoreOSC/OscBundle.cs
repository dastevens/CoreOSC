namespace CoreOSC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class OscBundle : OscPacket
    {
        private Timetag timetag;

        public ulong Timetag
        {
            get { return this.timetag.Tag; }
            set { this.timetag.Tag = value; }
        }

        public DateTime Timestamp
        {
            get { return this.timetag.Timestamp; }
            set { this.timetag.Timestamp = value; }
        }

        public List<OscMessage> Messages { get; } = new List<OscMessage>();

        public OscBundle(ulong timetag, params OscMessage[] args)
        {
            this.timetag = new Timetag(timetag);
            this.Messages.AddRange(args);
        }

        public override byte[] GetBytes()
        {
            string bundle = "#bundle";
            int bundleTagLen = Utils.AlignedStringLength(bundle);
            byte[] tag = SetULong(this.timetag.Tag);

            List<byte[]> outMessages = new List<byte[]>();
            foreach (OscMessage msg in this.Messages)
            {
                outMessages.Add(msg.GetBytes());
            }

            int len = bundleTagLen + tag.Length + outMessages.Sum(x => x.Length + 4);

            int i = 0;
            byte[] output = new byte[len];
            Encoding.ASCII.GetBytes(bundle).CopyTo(output, i);
            i += bundleTagLen;
            tag.CopyTo(output, i);
            i += tag.Length;

            foreach (byte[] msg in outMessages)
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