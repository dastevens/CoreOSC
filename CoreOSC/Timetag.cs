using System;

namespace CoreOSC
{
    public struct Timetag
    {
        public Timetag(ulong value)
        {
            this.Tag = value;
        }

        public static Timetag FromDateTime(DateTime value)
        {
            return new Timetag(Utils.DateTimeToTimetag(value));
        }

        public ulong Tag { get; }
    }
}