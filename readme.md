CoreOSC - OSC Library for .NET Standard 2.0
===================================


CoreOSC is a small library designed to make interacting with Open Sound Control easy (OSC). It provides the following features:

+ Produce an OSC Packet (messages and bundles) from .NET values.
+ Translate an OSC message (consisting of a sequence of bytes) into a .NET object.
+ Transmit OSC packets via UDP.
+ Receive OSC packets via UDP.

Roadmap
-------

Completed improvements to code base

1. Fix tests - OK
2. Tidy up code
   + Add StyleCop and fix issues
   + Make OSC classes immutable
   + Remove casts from/to OscPacket
3. Break out type switch into separate TypeConverter classes
4. Remove UDP code, use extension method to System.Net.Sockets.UdpClient
5. Update readme.md with new interface and examples
6. Release to nuget

Planned future improvements

7. Map OscFalse, OscTrue, OscInfinitum, OscNil etc. to false/true values
8. Automate nuget release with AppVeyor

History
--------

Forked from https://github.com/Dalesjo/CoreOSC
CoreOSC is forked and converted from [SharpOSC](https://github.com/ValdemarOrn/SharpOSC) made by [ValdermarOrn](https://github.com/ValdemarOrn)

Supported Types
---------------

[The following OSC types](http://opensoundcontrol.org/spec-1_0) are supported:

* i	- int32 (System.Int32)
* f	- float32 (System.Single)
* s	- OSC-string (System.String)
* b	- OSC-blob (System.Byte[])
* h	- 64 bit big-endian two's complement integer (System.Int64)
* t	- OSC-timetag (System.UInt64 / CoreOSC.Timetag)
* d	- 64 bit ("double") IEEE 754 floating point number (System.Double)
* S	- Alternate type represented as an OSC-string (for example, for systems that differentiate "symbols" from "strings") (CoreOSC.Symbol)
* c	- an ascii character, sent as 32 bits (System.Char)
* r	- 32 bit RGBA color (CoreOSC.RGBA)
* m	- 4 byte MIDI message. Bytes from MSB to LSB are: port id, status byte, data1, data2 (CoreOSC.Midi)
* T	- True. No bytes are allocated in the argument data. (System.Boolean)
* F	- False. No bytes are allocated in the argument data. (System.Boolean)
* N	- Nil. No bytes are allocated in the argument data. (null)
* I	- Infinitum. No bytes are allocated in the argument data. (Double.PositiveInfinity)

Unsupported Types
---------------

Note that nested arrays (arrays within arrays) are not supported, the OSC specification is unclear about whether that it is even allowed.

* [	- Indicates the beginning of an array. The tags following are for data in the Array until a close brace tag is reached. (System.Object[] / List\<object\>)
* ]	- Indicates the end of an array.

License
-------

CoreOSC is licensed under the MIT license. 

See License.txt

Using The Library
-----------------

The library is released on nuget.org (https://www.nuget.org/packages/CoreOSC/).

Using power shell, you can install it with this command:

```
Install-Package CoreOSC
```

For a .NET core project you can install it like this:

```
dotnet add package CoreOSC
```

CoreOSC should now be available to use in your code under that namespace "CoreOSC".

Example 1: Sending a message without arguments
----------------------------------------------

    using CoreOSC;
    using CoreOSC.IO;
    using System;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    namespace Example1
    {
        class Program
        {
            static async Task Main(string[] args)
            {
                using (var udpClient = new UdpClient("127.0.0.1", 57100))
                {
                    var message = new OscMessage(new Address("/example1"));

                    await udpClient.SendMessageAsync(message);
                }
            }
        }
    }

This example sends an OSC message to the local machine on port 57100 without any arguments to the address /example1.

Example 2: Adding arguments to a message
---------------------------------------------

```
   var message = new OscMessage(
        address: new Address("/example2"),
        arguments: new object[]
        {
            42,                         // i - int32 (System.Int32)
            3.14159F,                   // f - float32 (System.Single)
            "A string",                 // s - OSC-string (System.String)
            new byte[] { 1, 2, 3 },     // b - OSC-blob (System.Byte[])
            123456L,                    // h - 64 bit big-endian two's complement integer (System.Int64)
            new Timetag(123456U),       // t - OSC-timetag (System.UInt64 / CoreOSC.Timetag)
            3.14159D,                   // d - 64 bit ("double") IEEE 754 floating point number (System.Double)
            new Symbol("A symbol"),     // S - Alternate type represented as an OSC-string (for example, for systems that differentiate "symbols" from "strings") (CoreOSC.Symbol)
            'c',                        // c - an ascii character, sent as 32 bits(System.Char)
            new RGBA(255, 0, 0, 128),   // r - 32 bit RGBA color(CoreOSC.RGBA)
            new Midi(0, 0, 20, 64),     // m - 4 byte MIDI message.Bytes from MSB to LSB are: port id, status byte, data1, data2 (CoreOSC.Midi)
            OscTrue.True,               // T - True.No bytes are allocated in the argument data. (System.Boolean)
            OscFalse.False,             // F - False.No bytes are allocated in the argument data. (System.Boolean)
            OscNil.Nil,                 // N - Nil.No bytes are allocated in the argument data. (null)
            OscInfinitum.Infinitum,     // I - Infinitum.No bytes are allocated in the argument data. (Double.PositiveInfinity)
        });
```

The example above constructs an OscMessage with arguments. In the sample code, a message with address /example2 is constructed, and an argument of each supported type is added.
                    
Example 3: Receiving a Message
------------------------------

    using CoreOSC;
    using CoreOSC.IO;
    using System;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    namespace Example3
    {
        class Program
        {
            static async Task Main(string[] args)
            {
                using (var udpClient = new UdpClient("127.0.0.1", 57100))
                {
                    var response = await udpClient.ReceiveMessageAsync();

                    Console.WriteLine(response.Address.Value);
                    // Do something with the arguments, (depending on the address)
                    foreach (var argument in response.Arguments)
                    {
                        Console.WriteLine(argument.GetType());
                    }
                }
            }
        }
    }

Incoming messages can be accessed with the ReceiveMessageAsync extension method. The message has an address and an array of arguments.

In example 3, the address and then the type of each argument are printed to the console. In a real situation, the types of the arguments
will be known by the application. This would typically depend on the address of the message. You would then cast each argument to its
type to get the value of each argument.

Example 4: Receiving a message with arguments
---------------------------------------------

    using CoreOSC;
    using CoreOSC.IO;
    using System;
    using System.Linq;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    namespace Example4
    {
        class Program
        {
            static async Task Main(string[] args)
            {
                using (var udpClient = new UdpClient("127.0.0.1", 57100))
                {
                    var response = await udpClient.ReceiveMessageAsync();

                    if (response.Address.Value == "/example4")
                    {
                        Console.WriteLine("/example4");
                        var stringArgument = (string)response.Arguments.ElementAt(0);
                        var intArgument = (int)response.Arguments.ElementAt(1);
                        var falseArgument = (OscFalse)response.Arguments.ElementAt(2);
                        Console.WriteLine($"First string argument:  {stringArgument}");
                        Console.WriteLine($"Second int argument:    {intArgument}");
                        Console.WriteLine($"Third OscFalse argument:{falseArgument}");
                    }
                }
            }
        }
    }

In example 4, when a message with address /example4 is received, the application knows that the first argument is a string, the second is an
integer, and the third is an OSC False. In the sample above, the value of each message argument is  printed to the console.
