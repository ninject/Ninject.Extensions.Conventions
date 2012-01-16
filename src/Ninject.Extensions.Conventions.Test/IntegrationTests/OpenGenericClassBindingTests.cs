//-------------------------------------------------------------------------------
// <copyright file="OpenGenericClassBindingTests.cs" company="Ninject Project Contributors">
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
    using System.Linq;
    using FluentAssertions;

    using Ninject.Extensions.Conventions.Fakes.Interfaces;
    using Ninject.Extensions.Conventions.Fakes.OpenGenerics;
    using Xunit;

    public class OpenGenericClassBindingTests
    {
        [Fact]
        public void CanBeBoundToOpenGenericInterface()   
        {
            using (IKernel kernel = new StandardKernel())
            {
                kernel.Bind(
                    x => x.FromThisAssembly()
                          .SelectAllClasses().InNamespaceOf(typeof(OpenGenericClassWithManyInterfaces<>))
                          .BindToAllInterfaces());
                var instance = kernel.Get<IOpenGenericInterface<double>>();

                instance.Should().BeAssignableTo<OpenGenericClassWithManyInterfaces<double>>();
            }
        }

        [Fact]
        public void CanBeBoundToOpenGenericInterfaceOfPartiallyClosedBaseClass()
        {
            using (IKernel kernel = new StandardKernel())
            {
                kernel.Bind(
                    x => x.FromThisAssembly()
                          .SelectAllClasses().InNamespaceOf(typeof(OpenGenericClassWithManyInterfaces<>))
                          .BindToAllInterfaces());
                var instances = kernel.GetAll<IBaseOpenGenericInterface1<double>>();

                instances.Select(i => i.GetType()).Should().BeEquivalentTo(
                    typeof(OpenGenericClassWithManyInterfaces<double>),
                    typeof(GenericBaseClassWithManyInterfaces<double>));
            }
        }
        
        [Fact(Skip = "Not yet supported by Ninject Core.")]
        public void CanBeBoundToPartiallyClosedGenericInterface()
        {
            using (IKernel kernel = new StandardKernel())
            {
                kernel.Bind(
                    x => x.FromThisAssembly()
                          .Select(t => t == typeof(OpenGenericClassWithManyInterfaces<>))
                          .BindToAllInterfaces());
                var instance = kernel.Get<IBaseOpenGenericInterface<double, int>>();

                instance.Should().BeAssignableTo<OpenGenericClassWithManyInterfaces<double>>();
            }
        }
    }
}