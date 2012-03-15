//-------------------------------------------------------------------------------
// <copyright file="SpecialConfigurations.cs" company="Ninject Project Contributors">
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

namespace Ninject.Extensions.Conventions.IntegrationTests
{
    using FluentAssertions;
    using Ninject.Extensions.Conventions.Fakes;
    using Ninject.Modules;

    using Xunit;

    public class SpecialConfigurations
    {
        [Fact]
        public void ConfigurationCanBeOverridenForSelectedTypes()
        {
            using (IKernel kernel = new StandardKernel())
            {
                kernel.Bind(
                    x => x.FromThisAssembly()
                          .SelectAllClasses().InNamespaceOf<Foo>()
                          .BindAllInterfaces()
                          .Configure(c => c.Named("Default"))
                          .ConfigureFor<Foo>(c => c.Named("Foo")));
                
                var instance = kernel.Get<IFoo>("Foo");

                instance.Should().BeOfType<Foo>();
            }               
        }

        [Fact]
        public void DefaultConfigurationIsUsedIfNotOverridden()
        {
            using (IKernel kernel = new StandardKernel())
            {
                kernel.Bind(
                    x => x.FromThisAssembly()
                          .SelectAllClasses().InNamespaceOf<Foo>()
                          .BindAllInterfaces()
                          .Configure((c, s) => c.Named("Default").WithMetadata("type", s.Name))
                          .ConfigureFor<Foo>(c => c.Named("Foo")));

                var instance = kernel.Get<IFoo>(m => m.Get("type", string.Empty) == "Foo");

                instance.Should().BeOfType<Foo>();
            }
        }
  
        [Fact]
        public void DefaultConfigurationIsAppliedOtherwise()
        {
            using (IKernel kernel = new StandardKernel())
            {
                kernel.Bind(
                    x => x.FromThisAssembly()
                          .SelectAllClasses().InNamespaceOf<Foo>()
                          .BindAllInterfaces()
                          .Configure(c => c.Named("Default"))
                          .ConfigureFor<Foo>(c => c.Named("Foo")));

                var instance = kernel.Get<IMultipleInterfaceCrazyService>("Default");

                instance.Should().BeOfType<MultipleInterfaceCrazyService>();
            }
        }
    }
}