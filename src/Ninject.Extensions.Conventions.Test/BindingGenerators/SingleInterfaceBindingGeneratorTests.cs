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
    using Moq;

    using Ninject.Extensions.Conventions.BindingBuilder;
    using Ninject.Extensions.Conventions.Fakes;
    using Xunit;

    public class SingleInterfaceBindingGeneratorTests
    {
        private readonly IBindingGenerator testee;
        private readonly KernelMock kernelMock;
        private readonly Mock<IBindableTypeSelector> bindableInterfaceSelectorMock;

        public SingleInterfaceBindingGeneratorTests()
        {
            this.kernelMock = new KernelMock();
            this.bindableInterfaceSelectorMock = new Mock<IBindableTypeSelector>();
            var bindingGeneratorFactory = new BindingGeneratorFactory(this.bindableInterfaceSelectorMock.Object);
            this.testee = bindingGeneratorFactory.CreateSingleInterfaceBindingGenerator();
        }

        [Fact]
        public void ExceptionIsThrownIfMoreThanOneInterfaceExists()
        {
            var type = typeof(MultipleInterfaceCrazyService);
            var interfaces = new[] { typeof(IFoo), typeof(IBar) };
            this.bindableInterfaceSelectorMock.Setup(s => s.GetBindableInterfaces(type)).Returns(interfaces);

            Assert.Throws<InvalidOperationException>(() => this.testee.CreateBindings(type, this.kernelMock.Object).ToList());
        }

        [Fact]
        public void ExceptionIsThrownIfNoInterfaceExists()
        {
            var type = typeof(MultipleInterfaceCrazyService);
            var interfaces = new Type[0];
            this.bindableInterfaceSelectorMock.Setup(s => s.GetBindableInterfaces(type)).Returns(interfaces);

            Assert.Throws<InvalidOperationException>(() => this.testee.CreateBindings(type, this.kernelMock.Object).ToList());
        }
        
        [Fact]
        public void SingleInterfaceIsBound()
        {
            var type = typeof(MultipleInterfaceCrazyService);
            var interfaces = new[] { typeof(IFoo) };
            this.bindableInterfaceSelectorMock.Setup(s => s.GetBindableInterfaces(type)).Returns(interfaces);

            this.testee.CreateBindings(type, this.kernelMock.Object).ToList();

            this.kernelMock.VerifyAllBindingsCreated(interfaces, type);
        }

        [Fact]
        public void SyntaxFormAllBindingsAreReturned()
        {
            var type = typeof(MultipleInterfaceCrazyService);
            var interfaces = new[] { typeof(IFoo) };
            this.bindableInterfaceSelectorMock.Setup(s => s.GetBindableInterfaces(type)).Returns(interfaces);

            var syntax = this.testee.CreateBindings(type, this.kernelMock.Object).ToList();

            syntax.Should().Contain(this.kernelMock.ReturnedSyntax);
        }
    }
}
#endif