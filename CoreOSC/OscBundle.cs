namespace CoreOSC
{
    using System.Collections.Generic;

    public struct OscBundle
    {
        public OscBundle(Timetag timetag, IEnumerable<OscMessage> messages)
        {
            this.Timetag = timetag;
            this.Messages = messages;
        }

        public Timetag Timetag { get; }

        public IEnumerable<OscMessage> Messages { get; }
    }
}