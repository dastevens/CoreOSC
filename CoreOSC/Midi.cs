namespace CoreOSC
{
    public struct Midi
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="Midi"/> struct.
        /// </summary>
        /// <param name="port"></param>
        /// <param name="status"></param>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
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