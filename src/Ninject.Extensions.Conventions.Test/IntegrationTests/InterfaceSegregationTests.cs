//-------------------------------------------------------------------------------
// <copyright file="InterfaceSegregationTests.cs" company="Ninject Project Contributors">
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

    using Xunit;

    public class InterfaceSegregationTests
    {
        [Fact]
        public void BindToAllInterfaceReturnsSameInstanceForSameScope()
        {
            using (IKernel kernel = new StandardKernel())
            {
                kernel.Bind(
                    x => x.FromThisAssembly()
                          .SelectAllClasses()
                          .Where(t => t == typeof(Foo))
                          .BindAllInterfaces()
                          .Configure(c => c.InSingletonScope()));

                var instance1 = kernel.Get<IFoo>();
                var instance2 = kernel.Get<IFoo<int>>();

                instance1.Should().BeSameAs(instance2);
            }
        }
    
        [Fact]
        public void BindToDefaultInterfacesReturnsSameInstanceForSameScope()
        {
            using (IKernel kernel = new StandardKernel())
            {
                kernel.Bind(
                    x => x.FromThisAssembly()
                          .Select(t => t == typeof(Foo))
                          .BindDefaultInterfaces()
                          .Configure(c => c.InSingletonScope()));

                var instance1 = kernel.Get<IFoo>();
                var instance2 = kernel.Get<IFoo<int>>();

                instance1.Should().BeSameAs(instance2);
            }
        }

        [Fact]
        public void BindToSelectionReturnsSameInstanceForSameScope()
        {
            using (IKernel kernel = new StandardKernel())
            {
                kernel.Bind(
                    x => x.FromThisAssembly()
                          .Select(t => t == typeof(Foo))
                          .BindSelection((ts, ti) => ti)
                          .Configure(c => c.InSingletonScope()));

                var instance1 = kernel.Get<IFoo>();
                var instance2 = kernel.Get<IFoo<int>>();

                instance1.Should().BeSameAs(instance2);
            }
        }

        [Fact]
        public void BindToRegexReturnsSameInstanceForSameScope()
        {
            using (IKernel kernel = new StandardKernel())
            {
                kernel.Bind(
                    x => x.FromThisAssembly()
                          .Select(t => t == typeof(Foo))
                          .BindUsingRegex(".*")
                          .Configure(c => c.InSingletonScope()));

                var instance1 = kernel.Get<IFoo>();
                var instance2 = kernel.Get<IFoo<int>>();

                instance1.Should().BeSameAs(instance2);
            }
        }
    }
}