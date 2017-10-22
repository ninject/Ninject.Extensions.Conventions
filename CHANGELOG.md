# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [3.3.0] - 2017-10-23

### Added
 - Support .NET Standard 2.0
 - Marked CLS Compliant

### Changed
- Spelling: renamed "NonePublic" to "NonPublic"

### Fixed
- `GetNameWithoutGenericPart` Throws `SubString` exception [#26](https://github.com/ninject/Ninject.Extensions.Conventions/issues/26)

### Removed
 - .NET 3.5, .NET 4.0 and Silverlight

## [3.3.0-beta1]

### Added
 - Support .NET Standard 2.0

### Changed
- Spelling: renamed "NonePublic" to "NonPublic"

### Fixed
- `GetNameWithoutGenericPart` Throws `SubString` exception [#26](https://github.com/ninject/Ninject.Extensions.Conventions/issues/26)

### Removed
 - .NET 3.5, .NET 4.0 and Silverlight

## [3.2.1]

### Added
- Support for combination of assembly pattern matching and filter
  E.g.
  ```C#
  kernel.Bind(x => x
     .FromAssembliesMatching(IEnumerable<string> patterns, Predicate<Assembly> filter)
     ...);
  ```

## [3.2.0]

### Added
- Support for multiple configurations in one BindWith
  E.g.
  ```C#
  kernel.Bind(x => {
     x.FromThisAssembly().....;
     x.FromThisAssembly().....;
  }
  ```

## [3.0.0.0]
Completely reimplemented, this extension is not backward compatiple. See the documentation for more information