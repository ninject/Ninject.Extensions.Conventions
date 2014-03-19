//-------------------------------------------------------------------------------
// <copyright file="NormalClassBindingTests.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2013 Ninject Project Contributors
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

using Ninject.Extensions.Conventions.Fakes;

namespace Ninject.Extensions.Conventions.IntegrationTests
{
    using System.Linq;
    using FluentAssertions;

    using Ninject.Extensions.Conventions.Fakes.Interfaces;
    using Ninject.Extensions.Conventions.Fakes.NormalClasses;
    using Ninject.Extensions.Conventions.Fakes.OpenGenerics;
    using Xunit;

    public class NormalClassBindingTests
    {
        [Fact]
        public void CanBeBoundToNormalInterface()
        {
            using (IKernel kernel = new StandardKernel())
            {
                kernel.Bind(
                    x => x.FromThisAssembly()
                          .SelectAllClasses().InNamespaceOf<ClassWithManyInterfaces>()
                          .BindAllInterfaces());
                var instance = kernel.Get<INormalInterface>();

                instance.Should().BeAssignableTo<ClassWithManyInterfaces>();
            }
        }
        
        [Fact]
        public void CanBeBoundToClosedGenericInterface()
        {
            using (IKernel kernel = new StandardKernel())
            {
                kernel.Bind(
                    x => x.FromThisAssembly()
                          .SelectAllClasses().InNamespaceOf<ClassWithManyInterfaces>()
                          .BindAllInterfaces());
                var instance = kernel.Get<IClosedGenericInterface<int>>();

                instance.Should().BeAssignableTo<ClassWithManyInterfaces>();
            }
        }

        [Fact]
        public void CanBeBoundToNormalBaseType()
        {
            using (IKernel kernel = new StandardKernel())
            {
                kernel.Bind(
                    x => x.FromThisAssembly()
                          .SelectAllClasses().InNamespaceOf<ClassWithManyInterfaces>()
                          .BindSelection((ts, ti) => ti.Where(i => !i.IsInterface)));
                var instances = kernel.GetAll<object>();

                instances.Select(i => i.GetType()).Should().BeEquivalentTo(typeof(DefaultConvention), typeof(ClassWithManyInterfaces));
            }
        }

        [Fact]
        public void CanBeBoundToClosedGenericBaseType()
        {
            using (IKernel kernel = new StandardKernel())
            {
                kernel.Bind(
                    x => x.FromThisAssembly()
                          .SelectAllClasses().InNamespaceOf<ClassWithManyInterfaces>()
                          .BindSelection((ts, ti) => ti.Where(i => !i.IsInterface)));
                var instance = kernel.Get<GenericBaseClassWithManyInterfaces<int, int>>();

                instance.Should().BeAssignableTo<ClassWithManyInterfaces>();
            }
        }

        [Fact]
        public void CanHaveMultipleStatementsInBind()
        {
            using (IKernel kernel = new StandardKernel())
            {
                kernel.Bind(
                    x =>
                    {
                        x.FromThisAssembly()
                            .SelectAllClasses().InNamespaceOf<ClassWithManyInterfaces>()
                            .BindAllInterfaces();
                        x.FromThisAssembly()
                            .SelectAllClasses().InheritedFrom<IService>()
                            .BindAllInterfaces();
                    });

                var instance1 = kernel.Get<INormalInterface>();
                var instance2 = kernel.Get<IService>();


                instance1.Should().BeAssignableTo<ClassWithManyInterfaces>();
                instance2.Should().BeAssignableTo<MultipleInterfaceCrazyService>();
            }
        } 
    }
}