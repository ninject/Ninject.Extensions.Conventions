//-------------------------------------------------------------------------------
// <copyright file="DefaultInterfaceBindingGeneratorTests.cs" company="Ninject Project Contributors">
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
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Moq;

    using Ninject.Extensions.Conventions.BindingBuilder;
    using Ninject.Extensions.Conventions.Fakes;
    using Ninject.Syntax;

    using Xunit;

    public class DefaultInterfaceBindingGeneratorTests
    {
        private readonly IBindingGenerator testee;
        private readonly KernelMock kernelMock;
        private readonly Mock<IBindableTypeSelector> bindableInterfaceSelectorMock;
        private readonly Mock<IBindingCreator> multiBindingCreatorMock;

        public DefaultInterfaceBindingGeneratorTests()
        {
            this.kernelMock = new KernelMock();
            this.bindableInterfaceSelectorMock = new Mock<IBindableTypeSelector>();
            this.multiBindingCreatorMock = new Mock<IBindingCreator>();
            var bindingGeneratorFactory = new TestBindingGeneratorFactory(
                this.bindableInterfaceSelectorMock.Object, 
                this.multiBindingCreatorMock.Object);
            this.testee = bindingGeneratorFactory.CreateDefaultInterfaceBindingGenerator();
        }

        [Fact]
        public void InterfacesWithTheSameNameAreBound()
        {
            var type = typeof(MultipleInterfaceCrazyService);
            var interfaces = new[] { typeof(IFoo), typeof(IMultipleInterfaceCrazyService), typeof(IBar) };
            var expectedInterfaces = new[] { typeof(IMultipleInterfaceCrazyService) };
            this.bindableInterfaceSelectorMock.Setup(s => s.GetBindableInterfaces(type)).Returns(interfaces);

            this.testee.CreateBindings(type, this.kernelMock.Object);

            this.multiBindingCreatorMock.Verify(mbc => mbc.CreateBindings(
                this.kernelMock.Object,
                It.Is<IEnumerable<Type>>(t => t.SequenceEqual(expectedInterfaces)), 
                type));
        }
        
        [Fact]
        public void GenericInterfacesWithTheSameNameAreBound()
        {
            var type = typeof(ParentBar<int>.Bar<int>);
            var interfaces = new[] { typeof(IFoo), typeof(IBar), typeof(IBar<int>), typeof(IBar<>) };
            var expectedInterfaes = interfaces.Where(i => i.Name.StartsWith("IBar"));
            this.bindableInterfaceSelectorMock.Setup(s => s.GetBindableInterfaces(type)).Returns(interfaces);

            this.testee.CreateBindings(type, this.kernelMock.Object).ToList();

            this.multiBindingCreatorMock.Verify(mbc => mbc.CreateBindings(
                this.kernelMock.Object,
                It.Is<IEnumerable<Type>>(t => t.SequenceEqual(expectedInterfaes)),
                type));
        }

        [Fact]
        public void SyntaxFormAllBindingsAreReturned()
        {
            var type = typeof(MultipleInterfaceCrazyService);
            var interfaces = new[] { typeof(IFoo), typeof(IMultipleInterfaceCrazyService), typeof(IBar) };
            var expectedInterfaces = new[] { typeof(IMultipleInterfaceCrazyService) };
            var result = new[] { new Mock<IBindingWhenInNamedWithOrOnSyntax<object>>().Object };

            this.bindableInterfaceSelectorMock.Setup(s => s.GetBindableInterfaces(type)).Returns(interfaces);
            this.multiBindingCreatorMock.Setup(mbc => mbc.CreateBindings(
                this.kernelMock.Object,
                It.Is<IEnumerable<Type>>(t => t.SequenceEqual(expectedInterfaces)),
                type)).Returns(result);

            var syntax = this.testee.CreateBindings(type, this.kernelMock.Object).ToList();

            syntax.Should().BeEquivalentTo(result);
        }
    }
}
#endif