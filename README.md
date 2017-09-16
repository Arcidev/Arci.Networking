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
```csharp
var buffer = new ByteBuffer()
buffer.WriteBit(byteValue & 0x1);
buffer.WriteBit(byteValue & 0x10);
buffer.WriteBit(byteValue & 0x80);
buffer.WriteBit(true);
buffer.FlushBits(); // Writes bits into stream if they do not already form byte (8 bit writes)
```
#### PacketGuid
UInt64 value represented as 8 byte values. Only bytes that are not 0 will be written to stream. In byte stream before writing guid first you should write bit values to be able to determine which bytes are not 0.
```csharp
var guid = new PacketGuid(uint64value);
buffer.WriteGuidBitStreamInOrder(guid, 0, 1, 2, 3, 4); // Order is up to you
buffer.WriteBit(false); // Value can be add in between
buffer.WriteGuidBitStreamInOrder(guid, 7, 6, 5);
buffer.FlushBits(); // Required because we wrote 9 bits instead of 8
buffer.WriteGuidByteStreamInOrder(guid, 0, 1, 2, 3, 4, 5, 6, 7); // Order is up to you
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
var packet = new Packet(byteStream); // or new ByteBuffer(byteStream) if you do not want to use Packet
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
client.SendPacket(packet); // Send data to server
var packets = await tcpClient.ReceiveDataAsync(false); // Await collection of packets from server
```
#### Using Server
```csharp
var server = new Server(10751);
var tcpClient = await server.AcceptClientAsync(); // Await new connection
var packets = tcpClient.ReceiveData(false); // Await collection of packets from this client
tcpClient.SendPacket(packet); // Send data back to client
```
#### Aes
```csharp
var aes = new AesEncryptor() { PaddingMode = PaddingMode.PKCS7 };
var encryptedData = aes.Encrypt(packet.Data);
var decryptedData = aes.Decrypt(encryptedData);
client.Encryptor = aes; // you can pass aes encryptor to client and it will handle encryption by itself
```
#### RSA
```csharp
var rsa = new RsaEncryptor(RSAKey.RsaParams) { UseOAEPPadding = useOaepPadding };
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
