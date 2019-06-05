# Arci.Networking
Simple library for client-server network communication with as less dependencies as possible. Use packets to encapsulate your data which can be encrypted using either RSA or AES. For RSA you can choose to use padding scheme OAEP or not. For AES you can choose to generate a key of length 16, 24 or 32 bytes and use several padding modes like for example PKCS7. Samples included within solution.

## Nuget
[![NuGet](https://img.shields.io/nuget/v/Arci.Networking.svg?logo=nuget&style=flat-square)](https://www.nuget.org/packages/Arci.Networking)
[![NuGet downloads](https://img.shields.io/nuget/dt/Arci.Networking.svg?logo=nuget&style=flat-square)](https://www.nuget.org/packages/Arci.Networking)

	PM> Install-Package Arci.Networking
  
## Build Status
[![Build status](https://img.shields.io/travis/com/Arcidev/Arci.Networking.svg?logo=travis&style=flat-square)](https://travis-ci.com/Arcidev/Arci.Networking)
[![Build status](https://img.shields.io/appveyor/ci/Arcidev/arci-networking.svg?logo=appveyor&style=flat-square)](https://ci.appveyor.com/project/Arcidev/arci-networking)
[![Test status](https://img.shields.io/appveyor/tests/Arcidev/arci-networking.svg?logo=appveyor&style=flat-square)](https://ci.appveyor.com/project/Arcidev/arci-networking/build/tests)
[![Codacy](https://img.shields.io/codacy/grade/c6385a8834494ebcb0c53a5d4026f033.svg?logo=codacy&style=flat-square)](https://www.codacy.com/app/Arcidev/Arci.Networking?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Arcidev/Arci.Networking&amp;utm_campaign=Badge_Grade)

## Copyright
[![License](https://img.shields.io/github/license/Arcidev/Arci.Networking.svg?style=flat-square)](LICENSE.md)

## Documentation
[![Documentation](https://img.shields.io/badge/code-documented-brightgreen.svg?style=flat-square)](https://codedocs.xyz/Arcidev/Arci.Networking/)

## Usage
Basic usage of the library. You can find some basic example of a [client](https://github.com/Arcidev/Arci.Networking/tree/master/ClientSample) and a [server](https://github.com/Arcidev/Arci.Networking/tree/master/ServerSample) in the solution. For more advanced example you can check my [other project](https://github.com/Arcidev/Card-Game).

### Using serializer
Serializer allows to directly serialize/deserialize object into/from Packet object.
```csharp
// Marks class as serializable to packet with 1 as packet identifier
[PacketClass(1)]
public class MyObject
{
    // Marks property to be serialized in specified order
    [PacketProperty(1)]
    public SByte SByte { get; set; }

    [PacketProperty(2)]
    public UInt16 UInt16 { get; set; }

    [PacketProperty(3)]
    public DateTime DateTime { get; set; }
}

var obj = new MyObject();
// Object will be serialized into packet
var packet = obj.ToPacket();
// Deserializes object back to object of type MyObject
obj = packet.FromPacket<MyObject>();
```

### Without using serializer
You can choose not to use serializer and serialize/deserialize your objects manualy. Manual serialization is slightly faster (no requirements for reflection) and more flexible as the serializer does not support dynamic objects nor single types.

#### Writing byte based values

```csharp
var buffer = new ByteBuffer()
buffer.Write(uint32Value);
buffer.Write(int16Value);
buffer.Write(byteValue);
buffer.Write(stringValue);
buffer.Write(byteArrayValue);
buffer.Write(dateTimeValue);
```

#### Writing bit based values
Network stream can only work with byte values therefore we need to inform buffer that he needs to write values into stream as a whole byte when we're finished with bit writes (this is only necessary when bits does not form byte already e.g. number_of_bits_written % 8 != 0)
```csharp
var buffer = new ByteBuffer()
buffer.WriteBit(byteValue & 0x1);
buffer.WriteBit(byteValue & 0x10);
buffer.WriteBit(byteValue & 0x80);
buffer.WriteBit(true);
// Write bits into stream if they do not already form byte
buffer.FlushBits();
```

#### Writing values with specified number of bits

```csharp
var buffer = new ByteBuffer();
// Same as buffer.Write((UInt16)value)
// Flush not required because 16 % 8 == 0
buffer.WriteBits(uint32value, 16);

buffer.WriteBits(uint32Value, 2);
buffer.WriteBits(uint32Value, 4);
// Flush required because we did 2 + 4 bit writes and 6 % 8 != 0
buffer.FlushBits();
```

#### PacketGuid
UInt64 value represented as 8 byte values. Only bytes that are not 0 will be written to stream. In byte stream before writing guid first you should write bit values to be able to determine which bytes are not 0.
```csharp
var guid = new PacketGuid(uint64value);
// Order is up to you
buffer.WriteGuidBitStreamInOrder(guid, 0, 1, 2, 3, 4);
// Value can be added in between
buffer.WriteBit(false);
buffer.WriteGuidBitStreamInOrder(guid, 7, 6, 5);
// Flush required because we wrote 9 bits instead of 8
buffer.FlushBits();
buffer.WriteGuidByteStreamInOrder(guid, 0, 1, 2, 3, 4, 5, 6, 7);
// If you just want to save some space and do not care about the order
buffer.Write(guid);
```

#### Encapsulating with Packet

Extends ByteBuffer by adding an identifier to a stream
```csharp
var packet = new Packet(identifier);
packet.Write(uint32Value);
packet.WriteBit(byteValue & 0x10);
packet.WriteBit(true);
packet.FlushBits();
packet.Write(byteBuffer);
packet.Write(stringValue);
```

#### Builder
Only usable for Packet class
```csharp
var packet = new Packet(identifier).Builder()
.Write(uint32Value)
.WriteBit(byteValue & 0x10)
.WriteBit(true)
.FlushBits()
.Write(byteBuffer)
.Write(stringValue).Build();
```

#### Reading data

```csharp
var packet = new Packet(byteStream);
var uint32value = packet.ReadUInt32();
var guid = new PacketGuid();
packet.ReadGuidBitStreamInOrder(guid, 0, 1, 2, 3, 4, 7, 6, 5);
packet.ReadGuidByteStreamInOrder(guid, 0, 1, 2, 3, 4, 5, 6, 7);
guid = packet.ReadGuid();
var uint64value = (UInt64)guid;
var boolean = packet.ReadBit();
// Reading bit values reads the whole byte into memory and bits are being read from there.
// Necessary if you wish to start bit reading from a new byte.
packet.ClearUnflushedBits();
```

#### Using Client

```csharp
var client = await Client.CreateAsync("localhost", 10751);
// Send data to server
client.SendPacket(packet);
// Await collection of packets from server
var packets = await tcpClient.ReceiveDataAsync(false);
```

#### Using Server

```csharp
var server = new Server(10751);
// Await new connection
var tcpClient = await server.AcceptClientAsync();
// Await collection of packets from this client
var packets = tcpClient.ReceiveData(false);
// Send data back to client
tcpClient.SendPacket(packet); 
```

#### Aes

```csharp
var aes = new AesEncryptor() { PaddingMode = PaddingMode.PKCS7 };
var encryptedData = aes.Encrypt(packet.Data);
var decryptedData = aes.Decrypt(encryptedData);
// client can handle encryption if you set an encryptor
client.Encryptor = aes;
```

#### RSA

```csharp
var rsa = new RsaEncryptor(RSAKey.RsaParams);
var rsaEncryptedValue = rsa.Encrypt(packet.Data);
var rsaDecryptedValue = rsa.Decrypt(rsaEncryptedValue);
```

## Changes
See [Wiki](https://github.com/Arcidev/Arci.Networking/wiki) for a list of changes between versions

## Supported frameworks
- .NET Standard 2.0
