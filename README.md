# Arci.Networking
Simple library for client-server network communication with as less dependencies as possible. Use packets to encapsulate your data which can be encrypted using either RSA or AES. For RSA you can choose to use padding scheme OAEP or not. For AES you can choose to generate a key of length 16, 24 or 32 bytes and use several padding modes like for example PKCS7. Samples included within solution.

## Nuget
[![NuGet](https://img.shields.io/nuget/v/Arci.Networking.svg?style=flat-square)](https://www.nuget.org/packages/Arci.Networking)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Arci.Networking.svg?style=flat-square)](https://www.nuget.org/packages/Arci.Networking)

	PM> Install-Package Arci.Networking
  
## Build Status
[![Build status](https://img.shields.io/appveyor/ci/Arcidev/arci-networking.svg?style=flat-square)](https://ci.appveyor.com/project/Arcidev/arci-networking) (MSVC15)

## Copyright
[![license](https://img.shields.io/github/license/Arcidev/Arci.Networking.svg?style=flat-square)](LICENSE.md)

## Usage
Basic usage of the library. You can find some basic example of an [client](https://github.com/Arcidev/Arci.Networking/tree/master/ClientSample) and a [server](https://github.com/Arcidev/Arci.Networking/tree/master/ServerSample) in the solution. For more advanced example you can check my [other project](https://github.com/Arcidev/Card-Game).
#### Writing byte based values
```csharp
var buffer = new ByteBuffer()
buffer.Write(uint32Value);
buffer.Write(int16Value);
buffer.Write(byteValue);
buffer.Write(stringValue);
buffer.Write(byteArrayValue);
```
#### Writing bit based values
Network stream can only work with byte values therefore we need to inform buffer that he needs to write values into stream as a whole byte when we're finished with bit writes (this is only necessary when bits does not form byte already e.g. number_of_bits_written % 8 != 0
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
var uint64value = (UInt64)guid;
var boolean = packet.ReadBit();
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
- .NET 4.6.1
- .NET Standard 1.6
- .NET CoreApp 1.0
- Universal Windows Platform
