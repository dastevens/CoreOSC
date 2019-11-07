namespace CoreOSC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class OscMessage : OscPacket
    {
        public OscMessage(string address, params object[] args)
        {
            this.Address = address;
            this.Arguments = new List<object>();
            this.Arguments.AddRange(args);
        }

        public string Address { get; }

        public List<object> Arguments { get; }

        public override byte[] GetBytes()
        {
            var parts = new List<byte[]>();

            var currentList = this.Arguments;
            var argumentsIndex = 0;

            var typeString = ",";
            var i = 0;
            while (i < currentList.Count)
            {
                var arg = currentList[i];

                var type = (arg != null) ? arg.GetType().ToString() : "null";
                switch (type)
                {
                    case "System.Int32":
                        typeString += "i";
                        parts.Add(SetInt((int)arg));
                        break;

                    case "System.Single":
                        if (float.IsPositiveInfinity((float)arg))
                        {
                            typeString += "I";
                        }
                        else
                        {
                            typeString += "f";
                            parts.Add(SetFloat((float)arg));
                        }

                        break;

                    case "System.String":
                        typeString += "s";
                        parts.Add(SetString((string)arg));
                        break;

                    case "System.Byte[]":
                        typeString += "b";
                        parts.Add(SetBlob((byte[])arg));
                        break;

                    case "System.Int64":
                        typeString += "h";
                        parts.Add(SetLong((long)arg));
                        break;

                    case "System.UInt64":
                        typeString += "t";
                        parts.Add(SetULong((ulong)arg));
                        break;

                    case "CoreOSC.Timetag":
                        typeString += "t";
                        parts.Add(SetULong(((Timetag)arg).Tag));
                        break;

                    case "System.Double":
                        if (double.IsPositiveInfinity((double)arg))
                        {
                            typeString += "I";
                        }
                        else
                        {
                            typeString += "d";
                            parts.Add(SetDouble((double)arg));
                        }

                        break;

                    case "CoreOSC.Symbol":
                        typeString += "S";
                        parts.Add(SetString(((Symbol)arg).Value));
                        break;

                    case "System.Char":
                        typeString += "c";
                        parts.Add(SetChar((char)arg));
                        break;

                    case "CoreOSC.RGBA":
                        typeString += "r";
                        parts.Add(SetRGBA((RGBA)arg));
                        break;

                    case "CoreOSC.Midi":
                        typeString += "m";
                        parts.Add(SetMidi((Midi)arg));
                        break;

                    case "System.Boolean":
                        typeString += ((bool)arg) ? "T" : "F";
                        break;

                    case "null":
                        typeString += "N";
                        break;

                    // This part handles arrays. It points currentList to the array and resets i
                    // The array is processed like normal and when it is finished we replace
                    // currentList back with Arguments and continue from where we left off
                    case "System.Object[]":
                    case "System.Collections.Generic.List`1[System.Object]":
                        if (arg.GetType() == typeof(object[]))
                        {
                            arg = ((object[])arg).ToList();
                        }

                        if (this.Arguments != currentList)
                        {
                            throw new Exception("Nested Arrays are not supported");
                        }

                        typeString += "[";
                        currentList = (List<object>)arg;
                        argumentsIndex = i;
                        i = 0;
                        continue;

                    default:
                        throw new Exception("Unable to transmit values of type " + type);
                }

                i++;
                if (currentList != this.Arguments && i == currentList.Count)
                {
                    // End of array, go back to main Argument list
                    typeString += "]";
                    currentList = this.Arguments;
                    i = argumentsIndex + 1;
                }
            }

            var addressLen = (this.Address.Length == 0 || this.Address == null) ? 0 : Utils.AlignedStringLength(this.Address);
            var typeLen = Utils.AlignedStringLength(typeString);

            var total = addressLen + typeLen + parts.Sum(x => x.Length);

            var output = new byte[total];
            i = 0;

            Encoding.ASCII.GetBytes(this.Address).CopyTo(output, i);
            i += addressLen;

            Encoding.ASCII.GetBytes(typeString).CopyTo(output, i);
            i += typeLen;

            foreach (var part in parts)
            {
                part.CopyTo(output, i);
                i += part.Length;
            }

            return output;
        }
    }
}