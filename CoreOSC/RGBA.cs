namespace CoreOSC
{
    public struct RGBA
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RGBA"/> struct.
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <param name="alpha"></param>
        public RGBA(byte red, byte green, byte blue, byte alpha)
        {
            this.R = red;
            this.G = green;
            this.B = blue;
            this.A = alpha;
        }

        public byte R { get; }

        public byte G { get; }

        public byte B { get; }

        public byte A { get; }
    }
}