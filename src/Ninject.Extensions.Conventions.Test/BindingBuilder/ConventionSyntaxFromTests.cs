//-------------------------------------------------------------------------------
// <copyright file="ConventionSyntaxFromTests.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2011 Ninject Project Contributors
//   Authors: Remo Gloor (remo.gloor@gmail.com)
//           
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
//   you may not use this file except in compliance with one of the Licenses.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//   or
//       http://www.microsoft.com/opensource/licenses.mspx
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-------------------------------------------------------------------------------

#if !NO_MOQ
namespace Ninject.Extensions.Conventions.BindingBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Moq;

    using Ninject.Extensions.Conventions.Fakes.NormalClasses;

    using Xunit;

    public class ConventionSyntaxFromTests
    {
        private readonly Mock<IConventionBindingBuilder> conventionBindingBuilderMock;

        private readonly ConventionSyntax testee;

#if !NO_ASSEMBLY_SCANNING
        private readonly Mock<IAssemblyFinder> assemblyFinderMock;
#endif
        public ConventionSyntaxFromTests()
        {
            this.conventionBindingBuilderMock = new Mock<IConventionBindingBuilder>();
#if !NO_ASSEMBLY_SCANNING
            this.assemblyFinderMock = new Mock<IAssemblyFinder>();
            this.testee = new ConventionSyntax(this.conventionBindingBuilderMock.Object, this.assemblyFinderMock.Object, null, null);
#else
            this.testee = new ConventionSyntax(this.conventionBindingBuilderMock.Object, null, null);
#endif
        }

        [Fact]
        public void FromWithAssemblyParams_CallsBuilder_WithAnEnumerableContainingGivenAssemblies()
        {
            var assemblies = new[] { this.testee.GetType().Assembly, Assembly.GetCallingAssembly() };

            this.testee.From(assemblies[0], assemblies[1]);

            this.conventionBindingBuilderMock.Verify(b => b.SelectAllTypesFrom(IsMatchingSequence(assemblies)));
        }

        [Fact]
        public void FromWithMultipleAssemblies_CallsBuilder_WithTheGivenAssemblies()
        {
            var assemblies = new[] { this.testee.GetType().Assembly, Assembly.GetCallingAssembly() };

            this.testee.From(assemblies.AsEnumerable());

            this.conventionBindingBuilderMock.Verify(b => b.SelectAllTypesFrom(assemblies));
        }

        [Fact]
        public void FromThisAssembly_CallsBuilder_WithAnEnumerableContainingTheCallingAssembly()
        {
            var assembly = Assembly.GetExecutingAssembly();

            this.testee.FromThisAssembly();

            this.conventionBindingBuilderMock.Verify(b => b.SelectAllTypesFrom(IsMatchingSequence(assembly)));
        }

        [Fact]
        public void FromAssemblyContainingGeneric_CallsBuilder_WithAnEnumerableContainingTheAssemblyOfTheGivenType()
        {
            var assembly = typeof(List<int>).Assembly;

            this.testee.FromAssemblyContaining<List<int>>();

            this.conventionBindingBuilderMock.Verify(b => b.SelectAllTypesFrom(IsMatchingSequence(assembly)));
        }

        [Fact]
        public void FromAssemblyContainingWithParams_CallsBuilder_WithAnEnumerableContainingTheAssemblyOfTheGivenType()
        {
            var assembly1 = typeof(List<int>).Assembly;
            var assembly2 = typeof(DefaultConvention).Assembly;

            this.testee.FromAssemblyContaining(typeof(List<int>), typeof(DefaultConvention));

            this.conventionBindingBuilderMock.Verify(b => b.SelectAllTypesFrom(IsMatchingSequence(assembly1, assembly2)));
        }

        [Fact]
        public void FromAssemblyContainingWithMultipleTypes_CallsBuilder_WithAnEnumerableContainingTheAssemblyContainingTheGivenType()
        {
            var assembly1 = typeof(List<int>).Assembly;
            var assembly2 = typeof(DefaultConvention).Assembly;

            this.testee.FromAssemblyContaining((IEnumerable<Type>)new[] { typeof(List<int>), typeof(DefaultConvention) });

            this.conventionBindingBuilderMock.Verify(b => b.SelectAllTypesFrom(IsMatchingSequence(assembly1, assembly2)));
        }

#if !NO_ASSEMBLY_SCANNING        
        [Fact]
        public void FromWithAssemblyNameParams_CallsBuilder_WithTheAssembliesProvidedByTheFinder()
        {
            string[] assemblyNames = { "SomeName", "OtherName" };
            var assemblies = new[] { Assembly.GetCallingAssembly() };
            this.SetupFindAssemblies(assemblies, assemblyNames);

            this.testee.From(assemblyNames[0], assemblyNames[1]);

            this.conventionBindingBuilderMock.Verify(b => b.SelectAllTypesFrom(IsMatchingSequence(assemblies)));
        }

        [Fact]
        public void FromWithAssemblyNames_CallsBuilder_WithTheAssembliesProvidedByTheFinder()
        {
            string[] assemblyNames = { "SomeName" };
            var assemblies = new[] { Assembly.GetCallingAssembly() };
            this.SetupFindAssemblies(assemblies, assemblyNames);

            this.testee.From(assemblyNames.AsEnumerable());

            this.conventionBindingBuilderMock.Verify(b => b.SelectAllTypesFrom(assemblies));
        }

        [Fact]
        public void FromWithAssemblyNamesAndFilter_CallsBuilder_WithTheAssembliesProvidedByTheFinder()
        {
            string[] assemblyNames = { "SomeName" };
            var assemblies = new[] { Assembly.GetCallingAssembly() };
            var filter = new Predicate<Assembly>(a => true);
            this.SetupFindAssemblies(filter, assemblies, assemblyNames);

            this.testee.From(assemblyNames, filter);

            this.conventionBindingBuilderMock.Verify(b => b.SelectAllTypesFrom(assemblies));
        }

        [Fact]
        public void FromAssembliesInPath_CallsBuilder_WithAllAssembliesGivenByTheAssemblyFinder()
        {
            const string Path = "ThePath";
            var assemblyNames = new[] { "Assembly" };
            var assemblies = new[] { Assembly.GetCallingAssembly() };
            this.SetupFindAssembliesInPath(assemblyNames, Path);
            this.SetupFindAssemblies(assemblies, assemblyNames);

            this.testee.FromAssembliesInPath(Path);

            this.conventionBindingBuilderMock.Verify(b => b.SelectAllTypesFrom(IsMatchingSequence(assemblies)));
        }

        [Fact]
        public void FromAssembliesInPathTopDirectory_CallsBuilder_WithAllAssembliesGivenByTheAssemblyFinder()
        {
            const string Path = "ThePath";
            var assemblyNames = new[] { "Assembly" };
            var assemblies = new[] { Assembly.GetCallingAssembly() };
            this.SetupFindAssembliesInPath(assemblyNames, Path);
            this.SetupFindAssemblies(assemblies, assemblyNames);

            this.testee.FromAssembliesInPath(Path, System.IO.SearchOption.TopDirectoryOnly);

            this.conventionBindingBuilderMock.Verify(b => b.SelectAllTypesFrom(IsMatchingSequence(assemblies)));
        }

        [Fact]
        public void FromAssembliesInPathWithFilter_CallsBuilder_WithAllAssembliesGivenByTheAssemblyFinder()
        {
            const string Path = "ThePath";
            var assemblyNames = new[] { "Assembly" };
            var assemblies = new[] { Assembly.GetCallingAssembly() };
            var filter = new Predicate<Assembly>(a => true);

            this.SetupFindAssembliesInPath(assemblyNames, Path);
            this.SetupFindAssemblies(filter, assemblies, assemblyNames);

            this.testee.FromAssembliesInPath(Path, filter);

            this.conventionBindingBuilderMock.Verify(b => b.SelectAllTypesFrom(IsMatchingSequence(assemblies)));
        }

        [Fact]
        public void FromAssembliesInPathTopDirectoryWithFilter_CallsBuilder_WithAllAssembliesGivenByTheAssemblyFinder()
        {
            const string Path = "ThePath";
            var assemblyNames = new[] { "Assembly" };
            var assemblies = new[] { Assembly.GetCallingAssembly() };
            var filter = new Predicate<Assembly>(a => true);

            this.SetupFindAssembliesInPath(assemblyNames, Path);
            this.SetupFindAssemblies(filter, assemblies, assemblyNames);

            this.testee.FromAssembliesInPath(Path, System.IO.SearchOption.TopDirectoryOnly, filter);

            this.conventionBindingBuilderMock.Verify(b => b.SelectAllTypesFrom(IsMatchingSequence(assemblies)));
        }

        [Fact]
        public void FromAssembliesMatchingWithParams_CallsBuilder_WithAllAssembliesGivenByTheAssemblyFinder()
        {
            const string Pattern1 = "Pattern1";
            const string Pattern2 = "Pattern2";
            var assemblyNames = new[] { "Assembly" };
            var assemblies = new[] { Assembly.GetCallingAssembly() };

            this.SetupFindAssembliesMatching(assemblyNames, Pattern1, Pattern2);
            this.SetupFindAssemblies(assemblies, assemblyNames);

            this.testee.FromAssembliesMatching(Pattern1, Pattern2);

            this.conventionBindingBuilderMock.Verify(b => b.SelectAllTypesFrom(IsMatchingSequence(assemblies)));
        }

        [Fact]
        public void FromAssembliesMatching_CallsBuilder_WithAllAssembliesGivenByTheAssemblyFinder()
        {
            var patterns = new[] { "Pattern1", "Pattern2" };
            var assemblyNames = new[] { "Assembly" };
            var assemblies = new[] { Assembly.GetCallingAssembly() };

            this.SetupFindAssembliesMatching(assemblyNames, patterns);
            this.SetupFindAssemblies(assemblies, assemblyNames);

            this.testee.FromAssembliesMatching(patterns.AsEnumerable());

            this.conventionBindingBuilderMock.Verify(b => b.SelectAllTypesFrom(IsMatchingSequence(assemblies)));
        }

        [Fact]
        public void FromAssembliesMatchingWithFilter_CallBuilder_WithAllAssembliesGivenByTheAssemblyFinder()
        {
            Predicate<Assembly> filter = a => true;
            var patterns = new[] { "Pattern1", "Pattern2" };
            var assemblyNames = new[] { "Assembly" };
            var assemblies = new[] { Assembly.GetCallingAssembly() };

            this.SetupFindAssembliesMatching(assemblyNames, patterns);
            this.SetupFindAssemblies(filter, assemblies, assemblyNames);

            this.testee.FromAssembliesMatching(patterns.AsEnumerable(), filter);

            this.conventionBindingBuilderMock.Verify(b => b.SelectAllTypesFrom(IsMatchingSequence(assemblies)));
        }
#endif

#if !NO_SKIP_VISIBILITY
        [Fact]
        public void IncludingNonePublicTypes_CallsBuilder()
        {
            this.testee.IncludingNonePublicTypes();

            this.conventionBindingBuilderMock.Verify(b => b.IncludingNonePublicTypes());
        }
#endif

        private static IEnumerable<T> IsMatchingSequence<T>(params T[] values)
        {
#if !SILVERLIGHT_30 && !SILVERLIGHT_20 && !SILVERLIGHT_20
            return Match.Create<IEnumerable<T>>(value => value.SequenceEqual(values));
#else            
            return Match<IEnumerable<T>>.Create(value => value.SequenceEqual(values));
#endif
        }
        
#if !NO_ASSEMBLY_SCANNING
        private void SetupFindAssembliesMatching(IEnumerable<string> assemblyNames, params string[] patterns)
        {
            this.assemblyFinderMock.Setup(f => f.FindAssembliesMatching(IsMatchingSequence(patterns))).Returns(assemblyNames);
        }

        private void SetupFindAssembliesInPath(IEnumerable<string> assemblyNames, string path)
        {
            this.assemblyFinderMock.Setup(f => f.FindAssembliesInPath(path)).Returns(assemblyNames);
        }

        private void SetupFindAssemblies(IEnumerable<Assembly> assemblies, IEnumerable<string> assemblyNames)
        {
            this.assemblyFinderMock.Setup(f => f.FindAssemblies(assemblyNames, It.IsAny<Predicate<Assembly>>())).Returns(assemblies);
        }

        private void SetupFindAssemblies(Predicate<Assembly> filter, IEnumerable<Assembly> assemblies, IEnumerable<string> assemblyNames)
        {
            this.assemblyFinderMock.Setup(f => f.FindAssemblies(assemblyNames, filter)).Returns(assemblies);
        }
#endif
    }
}
#endif