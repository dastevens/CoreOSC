namespace CoreOSC
{
    public struct Midi
    {
        public Midi(byte port, byte status, byte data1, byte data2)
        {
            this.Port = port;
            this.Status = status;
            this.Data1 = data1;
            this.Data2 = data2;
        }

        public byte Port { get; }

        public byte Status { get; }

        public byte Data1 { get; }

        public byte Data2 { get; }
    }
}