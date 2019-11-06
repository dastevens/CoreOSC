namespace CoreOSC
{
    using System;

    public struct Timetag
    {
        public Timetag(ulong value)
        {
            this.Tag = value;
        }

        public ulong Tag { get; }

        public static Timetag FromDateTime(DateTime value)
        {
            return new Timetag(Utils.DateTimeToTimetag(value));
        }
    }
}