# Ninject.Extensions.Conventions [![NuGet Version](http://img.shields.io/nuget/v/Ninject.Extensions.Conventions.svg?style=flat)](https://www.nuget.org/packages/Ninject.Extensions.Conventions/) [![NuGet Downloads](http://img.shields.io/nuget/dt/Ninject.Extensions.Conventions.svg?style=flat)](https://www.nuget.org/packages/Ninject.Extensions.Conventions/)
Provides a configuration by convention extension for Ninject.

## Sample uses

### Binding default single interface
```CSharp
IKernel kernel = new StandardKernel();

kernel.Bind(x =>
    {
        x.FromThisAssembly() // Scans currently assembly
         .SelectAllClasses() // Retrieve all non-abstract classes
         .BindDefaultInterface(); // Binds the default interface to them;
    });
```

Default interface in this scenario means that if your interface is named IService, then the class named Service will be binded to it. However, the class SqlBasedService will not be binded and will fail to retrieve the implementation.

### Binding single interface
```CSharp
IKernel kernel = new StandardKernel();

kernel.Bind(x =>
    {
        x.FromThisAssembly() // Scans currently assembly
         .SelectAllClasses() // Retrieve all non-abstract classes
         .BindSingleInterface(); // Binds the default interface to them;
    });
```

Binding single interface means that you are expecting to have one implementation per classes found. This would bind SqlBasedService to IService as well as Service to IService but would not bind SqlBasedService and Service to an IService. 

You can find more details in [wiki page](https://github.com/ninject/Ninject.Extensions.Conventions/wiki).

## CI build status
[![Build Status](https://teamcity.bbv.ch/app/rest/builds/buildType:(id:bt25)/statusIcon)](http://teamcity.bbv.ch/viewType.html?buildTypeId=bt25&guest=1)
