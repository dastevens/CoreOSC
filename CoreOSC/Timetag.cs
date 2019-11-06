namespace CoreOSC
{
    using System;

    public struct Timetag
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Timetag"/> struct.
        /// </summary>
        /// <param name="value"></param>
        public Timetag(ulong value)
        {
            this.Tag = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Timetag"/> struct.
        /// </summary>
        /// <param name="value"></param>
        public Timetag(DateTime value)
        {
            this.Tag = Utils.DateTimeToTimetag(value);
        }

        public ulong Tag { get; }

        public DateTime Timestamp
        {
            get
            {
                return Utils.TimetagToDateTime(this.Tag);
            }
        }
    }
}