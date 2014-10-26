Version 3.2.1
-------------
- Add: Support for combination of assembly pattern matching and filter
  E.g.
```C#
  kernel.Bind(x => x
     .FromAssembliesMatching(IEnumerable<string> patterns, Predicate<Assembly> filter)
     ...);
```

Version 3.2.0
-------------
- Add: Support for multiple configurations in one BindWith
  E.g.
```C#
  kernel.Bind(x => {
     x.FromThisAssembly().....;
     x.FromThisAssembly().....;
  }
```

Version 3.0.0.0
---------------
Completely reimplemented, this extension is not backward compatiple. See the documentation for more information