# IocServiceStack

[![Gitter](https://badges.gitter.im/IocServiceStack/Lobby.svg)](https://gitter.im/IocServiceStack/IocServiceStack)   [![Build status](https://ci.appveyor.com/api/projects/status/bylhcbchnjas953q?svg=true)](https://ci.appveyor.com/project/rjinaga/iocservicestack)

IocServiceStack is a open source .NET dependency injection framework. IoC container allows to add multiple services for the same contract by name and dependecies can be added at different levels. This makes the concerns of application layers  separated, and configurable. so that client-specific logic can be plugged easily or inject the sepcific dependecies for specific client implementions. And also it offers several ways to inject dependencies. This framework contains the main container in which there are three types of internal containers such as root container, shared and dependec(ies)y container.


cies.

### Supports
- .NET Core 1.0 (.NET Standard 1.6)
- .NET Framework 4.6

## [NuGet](https://www.nuget.org/packages/IocServiceStack/2.0.0-rc-final)
```
PM> Install-Package IocServiceStack -Pre
```
[![NuGet Release](https://img.shields.io/badge/nuget-v2.0.0--rc--final-yellow.svg)](https://www.nuget.org/packages/IocServiceStack/2.0.0-rc-final)

## Documentation 

- [http://www.iocservicestack.net](http://www.iocservicestack.net)
- [https://github.com/rjinaga/IocServiceStack/wiki](https://github.com/rjinaga/IocServiceStack/wiki)
