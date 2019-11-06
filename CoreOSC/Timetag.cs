namespace CoreOSC
{
    using System;

    public struct Timetag
    {
        public ulong Tag;

        public DateTime Timestamp
        {
            get
            {
                return Utils.TimetagToDateTime(this.Tag);
            }
            set
            {
                this.Tag = Utils.DateTimeToTimetag(value);
            }
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
            set
            {
                this.Tag = (this.Tag & 0xFFFFFFFF00000000) + (uint)(value * 0xFFFFFFFF);
            }
        }

        public Timetag(ulong value)
        {
            this.Tag = value;
        }

        public Timetag(DateTime value)
        {
            this.Tag = 0;
            this.Timestamp = value;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Timetag))
            {
                if (this.Tag == ((Timetag)obj).Tag)
                    return true;
                else
                    return false;
            }
            else if (obj.GetType() == typeof(ulong))
            {
                if (this.Tag == ((ulong)obj))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public static bool operator ==(Timetag a, Timetag b)
        {
            if (a.Equals(b))
                return true;
            else
                return false;
        }

        public static bool operator !=(Timetag a, Timetag b)
        {
            if (a.Equals(b))
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return (int)(((uint)(this.Tag >> 32) + (uint)(this.Tag & 0x00000000FFFFFFFF)) / 2);
        }
    }
}