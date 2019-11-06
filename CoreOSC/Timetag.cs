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

            //set
            //{
            //    this.Tag = Utils.DateTimeToTimetag(value);
            //}
        }

        /// <summary>
        /// Gets or sets the fraction of a second in the timestamp. the double precision number is multiplied by 2^32
        /// giving an accuracy down to about 230 picoseconds ( 1/(2^32) of a second)
        /// </summary>
        public double Fraction
        {
            get
            {
                return Utils.TimetagToFraction(this.Tag);
            }

            //set
            //{
            //    this.Tag = (this.Tag & 0xFFFFFFFF00000000) + (uint)(value * 0xFFFFFFFF);
            //}
        }
    }
}