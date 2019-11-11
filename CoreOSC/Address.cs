namespace CoreOSC
{
    public struct Address
    {
        public Address(string value)
        {
            this.Value = value;
        }

        public string Value { get; }
    }
}