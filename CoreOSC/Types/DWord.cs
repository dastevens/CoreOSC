namespace CoreOSC.Types
{
    using System.Collections.Generic;
    using System.Linq;

    public struct DWord
    {
        public DWord(IEnumerable<byte> bytes)
            : this(bytes.Take(4).ToArray())
        {
        }

        public DWord(byte[] bytes)
            : this(
                  bytes.Length > 0 ? bytes[0] : (byte)0,
                  bytes.Length > 1 ? bytes[1] : (byte)0,
                  bytes.Length > 2 ? bytes[2] : (byte)0,
                  bytes.Length > 3 ? bytes[3] : (byte)0
                  )
        {
        }

        public DWord(byte byte0, byte byte1, byte byte2, byte byte3)
        {
            Byte0 = byte0;
            Byte1 = byte1;
            Byte2 = byte2;
            Byte3 = byte3;
        }

        public byte Byte0 { get; }

        public byte Byte1 { get; }

        public byte Byte2 { get; }

        public byte Byte3 { get; }

        public byte[] Bytes => new[] { Byte0, Byte1, Byte2, Byte3 };

        public DWord Reverse()
        {
            return new DWord(Byte3, Byte2, Byte1, Byte0);
        }
    }
}
