## Installation

To install from the Package Manager Console, just type the following:

    Install-Package Ninject.extensions.conventions

## Sample uses

# Binding default single interface
```CSharp
IKernel kernel = new StandardKernel();

kernel.Bind(x =>
    {
        x.FromThisAssembly() // Scans currently assembly
         .SelectAllClasses() // Retrieve all non-abstract classes
         .BindDefaultInterface() // Binds the default interface to them;
    });
```

Default interface in this scenario means that if your interface is named IService, then the class named Service will be binded to it. However, the class SqlBasedService will not be binded and will fail to retrieve the implementation.

# Binding single interface
```CSharp
IKernel kernel = new StandardKernel();

kernel.Bind(x =>
    {
        x.FromThisAssembly() // Scans currently assembly
         .SelectAllClasses() // Retrieve all non-abstract classes
         .BindSingleInterface() // Binds the default interface to them;
    });
```

Binding single interface means that you are expecting to have one implementation per classes found. This would bind SqlBasedService to IService as well as Service to IService but would not bind SqlBasedService and Service to an IService. 
