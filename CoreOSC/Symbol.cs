namespace CoreOSC
{
    public class Symbol
    {
        public string Value;

        public Symbol()
        {
            this.Value = "";
        }

        public Symbol(string value)
        {
            this.Value = value;
        }

        public override string ToString()
        {
            return this.Value;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Symbol))
            {
                if (this.Value == ((Symbol)obj).Value)
                    return true;
                else
                    return false;
            }
            else if (obj.GetType() == typeof(string))
            {
                if (this.Value == ((string)obj))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public static bool operator ==(Symbol a, Symbol b)
        {
            if (a.Equals(b))
                return true;
            else
                return false;
        }

        public static bool operator !=(Symbol a, Symbol b)
        {
            if (!a.Equals(b))
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }
    }
}