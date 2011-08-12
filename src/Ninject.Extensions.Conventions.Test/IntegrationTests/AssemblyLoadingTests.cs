//-------------------------------------------------------------------------------
// <copyright file="AssemblyLoadingTests.cs" company="Ninject Project Contributors">
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
    using System.Reflection;
    using FluentAssertions;
    using Ninject.Extensions.Conventions.Extensions;
    using Ninject.Extensions.Conventions.Fakes.Interfaces;
    using Ninject.Extensions.Conventions.Fakes.NormalClasses;
    using Xunit;

    public class AssemblyLoadingTests
    {
        [Fact]
        public void AssemblyCanBeSpecifiedUsingFrom()
        {
            using (IKernel kernel = new StandardKernel())
            {
                kernel.Bind(
                    x => x.From(Assembly.GetExecutingAssembly())
                          .SelectAllTypes()
                          .BindToDefaultInterface());
                var instance = kernel.Get<IDefaultConvention>();

                instance.Should().NotBeNull();
                instance.Should().BeOfType<DefaultConvention>();
            }
        }
    
        [Fact]
        public void AssemblyCanBeSpecifiedUsingThisAssembly()
        {
            using (IKernel kernel = new StandardKernel())
            {
                kernel.Bind(
                    x => x.FromThisAssembly()
                          .SelectAllTypes()
                          .BindToDefaultInterface());
                var instance = kernel.Get<IDefaultConvention>();

                instance.Should().NotBeNull();
                instance.Should().BeOfType<DefaultConvention>();
            }
        }

#if !NO_ASSEMBLY_SCANNING
        [Fact]
        public void AssemblyCanBeSpecifiedUsingAssemblyFileName()
        {
            using (IKernel kernel = new StandardKernel())
            {
                kernel.Bind(
                    x => x.From("TestPlugin.dll")
                          .SelectAllTypes()
                          .BindToDefaultInterface());
                var instance = kernel.Get<IDefaultConvention>();

                instance.Should().NotBeNull();
                instance.Name.Should().Be("DefaultConventionFromPlugin");
            }
        }

        [Fact]
        public void AssemblyCanBeSpecifiedUsingAssemblyName()
        {
            using (IKernel kernel = new StandardKernel())
            {
                kernel.Bind(
                    x => x.From("TestPlugin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
                          .SelectAllTypes()
                          .BindToDefaultInterface());
                var instance = kernel.Get<IDefaultConvention>();

                instance.Should().NotBeNull();
                instance.Name.Should().Be("DefaultConventionFromPlugin");
            }
        }
#endif
    }
}