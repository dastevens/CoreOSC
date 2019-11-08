namespace CoreOSC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CoreOSC.Types;

    public abstract class OscPacket
    {
        public static OscPacket GetPacket(byte[] oscData)
        {
            if (oscData[0] == '#')
            {
                return ParseBundle(oscData);
            }
            else
            {
                return ParseMessage(oscData);
            }
        }

        public abstract byte[] GetBytes();

        /// <summary>
        /// Takes in an OSC bundle package in byte form and parses it into a more usable OscBundle object.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>Message containing various arguments and an address.</returns>
        private static OscMessage ParseMessage(byte[] msg)
        {
            var index = 0;

            string address = null;
            var types = new char[0];
            var arguments = new List<object>();
            var mainArray = arguments; // used as a reference when we are parsing arrays to get the main array back

            // Get address
            address = GetAddress(msg, index);
            index += msg.FirstIndexAfter(address.Length, x => x == ',');

            if (index % 4 != 0)
            {
                throw new Exception("Misaligned OSC Packet data. Address string is not padded correctly and does not align to 4 byte interval");
            }

            // Get type tags
            types = GetTypes(msg, index);
            index += types.Length;

            while (index % 4 != 0)
            {
                index++;
            }

            var commaParsed = false;

            foreach (var type in types)
            {
                // skip leading comma
                if (type == ',' && !commaParsed)
                {
                    commaParsed = true;
                    continue;
                }

                switch (type)
                {
                    case '\0':
                        break;

                    case 'i':
                        var intVal = GetInt(msg, index);
                        arguments.Add(intVal);
                        index += 4;
                        break;

                    case 'f':
                        var floatVal = GetFloat(msg, index);
                        arguments.Add(floatVal);
                        index += 4;
                        break;

                    case 's':
                        var stringVal = GetString(msg, index);
                        arguments.Add(stringVal);
                        index += stringVal.Length;
                        break;

                    case 'b':
                        var blob = GetBlob(msg, index);
                        arguments.Add(blob);
                        index += 4 + blob.Length;
                        break;

                    case 'h':
                        var hval = GetLong(msg, index);
                        arguments.Add(hval);
                        index += 8;
                        break;

                    case 't':
                        var sval = GetULong(msg, index);
                        arguments.Add(new Timetag(sval));
                        index += 8;
                        break;

                    case 'd':
                        var dval = GetDouble(msg, index);
                        arguments.Add(dval);
                        index += 8;
                        break;

                    case 'S':
                        var symbolVal = GetString(msg, index);
                        arguments.Add(new Symbol(symbolVal));
                        index += symbolVal.Length;
                        break;

                    case 'c':
                        var cval = GetChar(msg, index);
                        arguments.Add(cval);
                        index += 4;
                        break;

                    case 'r':
                        var rgbaval = GetRGBA(msg, index);
                        arguments.Add(rgbaval);
                        index += 4;
                        break;

                    case 'm':
                        var midival = GetMidi(msg, index);
                        arguments.Add(midival);
                        index += 4;
                        break;

                    case 'T':
                        arguments.Add(true);
                        break;

                    case 'F':
                        arguments.Add(false);
                        break;

                    case 'N':
                        arguments.Add(null);
                        break;

                    case 'I':
                        arguments.Add(double.PositiveInfinity);
                        break;

                    case '[':
                        if (arguments != mainArray)
                        {
                            throw new Exception("SharopOSC does not support nested arrays");
                        }

                        arguments = new List<object>(); // make arguments point to a new object array
                        break;

                    case ']':
                        mainArray.Add(arguments); // add the array to the main array
                        arguments = mainArray; // make arguments point back to the main array
                        break;

                    default:
                        throw new Exception("OSC type tag '" + type + "' is unknown.");
                }

                while (index % 4 != 0)
                {
                    index++;
                }
            }

            return new OscMessage(address, arguments.ToArray());
        }

        /// <summary>
        /// Takes in an OSC bundle package in byte form and parses it into a more usable OscBundle object.
        /// </summary>
        /// <param name="bundle"></param>
        /// <returns>Bundle containing elements and a timetag.</returns>
        private static OscBundle ParseBundle(byte[] bundle)
        {
            var messages = new List<OscMessage>();

            var index = 0;

            var bundleTag = Encoding.ASCII.GetString(bundle.SubArray(0, 8));
            index += 8;

            var timetag = GetULong(bundle, index);
            index += 8;

            if (bundleTag != "#bundle\0")
            {
                throw new Exception("Not a bundle");
            }

            while (index < bundle.Length)
            {
                var size = GetInt(bundle, index);
                index += 4;

                var messageBytes = bundle.SubArray(index, size);
                var message = ParseMessage(messageBytes);

                messages.Add(message);

                index += size;
                while (index % 4 != 0)
                {
                    index++;
                }
            }

            var output = new OscBundle(timetag, messages.ToArray());
            return output;
        }

        private static string GetAddress(byte[] msg, int index)
        {
            var address = string.Empty;
            var chars = Encoding.UTF8.GetChars(msg);

            for (var i = index; i < chars.Length; i++)
            {
                if (chars[i] == ',')
                {
                    address = string.Join(string.Empty, chars.SubArray(index, i - index));
                    break;
                }
            }

            return address.Replace("\0", string.Empty);
        }

        private static char[] GetTypes(byte[] msg, int index)
        {
            var i = index + 4;
            char[] types = null;

            for (; i <= msg.Length; i += 4)
            {
                if (msg[i - 1] == 0)
                {
                    types = Encoding.ASCII.GetChars(msg.SubArray(index, i - index));
                    break;
                }
            }

            if (i >= msg.Length && types == null)
            {
                throw new Exception("No null terminator after type string");
            }

            return types;
        }

        private static int GetInt(byte[] msg, int index)
        {
            var dWords = new BytesConverter().Serialize(msg.Skip(index)).ToArray();
            new Types.IntConverter().Deserialize(dWords, out var value);
            return value;
        }

        private static float GetFloat(byte[] msg, int index)
        {
            var dWords = new BytesConverter().Serialize(msg.Skip(index)).ToArray();
            new Types.FloatConverter().Deserialize(dWords, out var value);
            return value;
        }

        private static string GetString(byte[] msg, int index)
        {
            var dWords = new BytesConverter().Serialize(msg.Skip(index)).ToArray();
            new Types.StringConverter().Deserialize(dWords, out var value);
            return value;
        }

        private static byte[] GetBlob(byte[] msg, int index)
        {
            var dWords = new BytesConverter().Serialize(msg.Skip(index)).ToArray();
            new Types.BlobConverter().Deserialize(dWords, out var value);
            return value.ToArray();
        }

        private static ulong GetULong(byte[] msg, int index)
        {
            var dWords = new BytesConverter().Serialize(msg.Skip(index)).ToArray();
            new Types.ULongConverter().Deserialize(dWords, out var value);
            return value;
        }

        private static long GetLong(byte[] msg, int index)
        {
            var dWords = new BytesConverter().Serialize(msg.Skip(index)).ToArray();
            new Types.LongConverter().Deserialize(dWords, out var value);
            return value;
        }

        private static double GetDouble(byte[] msg, int index)
        {
            var dWords = new BytesConverter().Serialize(msg.Skip(index)).ToArray();
            new Types.DoubleConverter().Deserialize(dWords, out var value);
            return value;
        }

        private static char GetChar(byte[] msg, int index)
        {
            var dWords = new BytesConverter().Serialize(msg.Skip(index)).ToArray();
            new Types.CharConverter().Deserialize(dWords, out var value);
            return value;
        }

        private static RGBA GetRGBA(byte[] msg, int index)
        {
            var dWords = new BytesConverter().Serialize(msg.Skip(index)).ToArray();
            new Types.RGBAConverter().Deserialize(dWords, out var value);
            return value;
        }

        private static Midi GetMidi(byte[] msg, int index)
        {
            var dWords = new BytesConverter().Serialize(msg.Skip(index)).ToArray();
            new Types.MidiConverter().Deserialize(dWords, out var value);
            return value;
        }

        protected static byte[] SetInt(int value)
        {
            return new Types.IntConverter().Serialize(value).First().Bytes;
        }

        protected static byte[] SetFloat(float value)
        {
            return new Types.FloatConverter().Serialize(value).First().Bytes;
        }

        protected static byte[] SetString(string value)
        {
            return new StringConverter().Serialize(value)
                .SelectMany(dWord => dWord.Bytes)
                .ToArray();
        }

        protected static byte[] SetBlob(byte[] value)
        {
            return new BlobConverter().Serialize(value)
                .SelectMany(dWord => dWord.Bytes)
                .ToArray();
        }

        protected static byte[] SetLong(long value)
        {
            return new LongConverter().Serialize(value)
                .SelectMany(dWord => dWord.Bytes)
                .ToArray();
        }

        protected static byte[] SetULong(ulong value)
        {
            return new ULongConverter().Serialize(value)
                .SelectMany(dWord => dWord.Bytes)
                .ToArray();
        }

        protected static byte[] SetDouble(double value)
        {
            return new DoubleConverter().Serialize(value)
                .SelectMany(dWord => dWord.Bytes)
                .ToArray();
        }

        protected static byte[] SetChar(char value)
        {
            return new Types.CharConverter().Serialize(value).First().Bytes;
        }

        protected static byte[] SetRGBA(RGBA value)
        {
            return new Types.RGBAConverter().Serialize(value).First().Bytes;
        }

        protected static byte[] SetMidi(Midi value)
        {
            return new Types.MidiConverter().Serialize(value).First().Bytes;
        }
    }
}