//-------------------------------------------------------------------------------
// <copyright file="AssemblyFinderTests.cs" company="Ninject Project Contributors">
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
#if !NO_ASSEMBLY_SCANNING
namespace Ninject.Extensions.Conventions.BindingBuilder
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using FluentAssertions;

    using Moq;

    using Ninject.Modules;

    using Xunit;

    public class AssemblyFinderTests
    {
        private readonly Mock<IAssemblyNameRetriever> assemblyNameRetrieverMock;

        private readonly AssemblyFinder testee;

        public AssemblyFinderTests()
        {
            this.assemblyNameRetrieverMock = new Mock<IAssemblyNameRetriever>();
            this.testee = new AssemblyFinder(this.assemblyNameRetrieverMock.Object);
        } 

        [Fact]
        public void FindAssembliesReturnsAListOfTheRequestedAssemblies()
        {
            var requestedAssemblies = new[] { "1", "2" };
            Predicate<Assembly> filter = a => true;
            var expectedAssembly = Assembly.GetExecutingAssembly();
            
            this.assemblyNameRetrieverMock
                .Setup(r => r.GetAssemblyNames(requestedAssemblies, filter))
                .Returns(new[] { expectedAssembly.GetName() });

            var assemblies = this.testee.FindAssemblies(requestedAssemblies, filter);

            assemblies.Should().BeEquivalentTo(expectedAssembly);
        }

        [Fact]
        public void FindAssembliesInPathReturnsTheAssemblyNamesOfTheMatchingAssmebliesInTheGivenPath()
        {
            var expectedAssembly = Assembly.GetExecutingAssembly();
            string path = new FileInfo(expectedAssembly.Location).DirectoryName;
            
            var assemblies = this.testee.FindAssembliesInPath(path);

            assemblies.Select(a => a.ToLower()).Should().Contain(expectedAssembly.Location.ToLower());
        }
    
        [Fact]
        public void FindAssembliesMatchingReturnsAllFileNamesOfMatchingFilesInTheCurrentDirectory()
        {
            var expectedAssembly1 = Assembly.GetExecutingAssembly();
            var expectedAssembly2 = this.testee.GetType().Assembly;

            var assemblies = this.testee.FindAssembliesMatching(new[] { "Ninject.Extensions.Conventions*.dll" })
                .Where(n => !(n.Contains("Silverlight") || n.Contains("WindowsPhone")));

            assemblies.Select<string, string>(GetFileName).Should()
                .BeEquivalentTo(new[] { expectedAssembly1, expectedAssembly2 }.Select(a => GetFileName(a.Location)));
        }

        private static string GetFileName(string path)
        {
            return new FileInfo(path).Name.ToLower();
        }
    }
}
#endif
#endif