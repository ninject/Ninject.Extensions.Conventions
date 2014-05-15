//-------------------------------------------------------------------------------
// <copyright file="SingleInterfaceBindingGeneratorTests.cs" company="Ninject Project Contributors">
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

#if !SILVERLIGHT_30 && !SILVERLIGHT_20 && !NO_MOQ && !NO_GENERIC_MOQ
namespace Ninject.Extensions.Conventions.BindingGenerators
{
    using System;
    using System.Linq;
    using FluentAssertions;

    using Ninject.Extensions.Conventions.BindingBuilder;
    using Ninject.Extensions.Conventions.Fakes;
    using Xunit;

    public class SpecificTypesBindingGeneratorTests
    {
        private static readonly Type[] ServiceTypes = { typeof(IFoo), typeof(IBar), typeof(IService) };

        private readonly IBindingGenerator testee;
        private readonly KernelMock kernelMock;
        
        public SpecificTypesBindingGeneratorTests()
        {
            this.kernelMock = new KernelMock();
            this.testee = new BindingGeneratorFactory(null).CreateSpecificTypesBindingGenerator(ServiceTypes);
        }

        [Fact]
        public void ExceptionIsThrownWhenTypeDoesNotImplementServiceType()
        {
            var type = typeof(Foo);

            var exception = Assert.Throws<InvalidOperationException>(() => this.testee.CreateBindings(type, this.kernelMock.Object).ToList());

            exception.Message.Should()
                .Contain(type.FullName)
                .And.Contain(typeof(IBar).FullName)
                .And.Contain(typeof(IService).FullName)
                .And.NotContain(typeof(IFoo).FullName);
        }

        [Fact]
        public void AllServiceTypesAreBound()
        {
            var type = typeof(MultipleInterfaceCrazyService);

            this.testee.CreateBindings(type, this.kernelMock.Object).ToList();

            this.kernelMock.VerifyBindingCreated(ServiceTypes, type);
        }

        [Fact]
        public void SyntaxForBindingIsReturned()
        {
            var type = typeof(MultipleInterfaceCrazyService);

            var syntax = this.testee.CreateBindings(type, this.kernelMock.Object).ToList();

            syntax.Should().Contain(this.kernelMock.ReturnedSyntax);
        }
    }
}
#endif