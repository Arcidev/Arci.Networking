# Arci.Networking
Simple library for client-server network communication with as less dependencies as possible. Use packets to encapsulate your data which can be encrypted using either RSA or AES. For RSA you can choose to use padding scheme OAEP or not. For AES you can choose to generate a key of length 16, 24 or 32 bytes and use several padding modes like for example PKCS7. Samples included within project.

## Nuget
[![NuGet](https://img.shields.io/nuget/v/Arci.Networking.svg?style=flat-square)](https://www.nuget.org/packages/Arci.Networking)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Arci.Networking.svg?style=flat-square)](https://www.nuget.org/packages/Arci.Networking)

	PM> Install-Package Arci.Networking
  
## Build Status
[![Build status](https://img.shields.io/appveyor/ci/Arcidev/arci-networking.svg?style=flat-square)](https://ci.appveyor.com/project/Arcidev/arci-networking) <span align="center">(MSVC15)</span>

## Copyright
[![license](https://img.shields.io/github/license/Arcidev/Arci.Networking.svg?style=flat-square)](LICENSE.md)

## Changes
See [Wiki](https://github.com/Arcidev/Arci.Networking/wiki) for a list of changes between versions

## Supported frameworks
- .NET 4.6.1
- .NET Standard 1.6
- .NET CoreApp 1.0
- Universal Windows Platform
