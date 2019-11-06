namespace CoreOSC
{
    public struct RGBA
    {
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