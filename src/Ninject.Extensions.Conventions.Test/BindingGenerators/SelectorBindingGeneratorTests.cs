//-------------------------------------------------------------------------------
// <copyright file="SelectorBindingGeneratorTests.cs" company="Ninject Project Contributors">
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
    using Ninject.Extensions.Conventions.Fakes.Interfaces;
    using Ninject.Extensions.Conventions.Fakes.NormalClasses;
    using Ninject.Syntax;

    using Xunit;

    public class SelectorBindingGeneratorTests
    {
        private readonly KernelMock kernelMock;
        private readonly Mock<IBindableTypeSelector> bindableInterfaceSelectorMock;
        private readonly BindingGeneratorFactory bindingGeneratorFactory;
        private readonly Mock<IBindingCreator> multiBindingCreatorMock;

        public SelectorBindingGeneratorTests()
        {
            this.kernelMock = new KernelMock();
            this.bindableInterfaceSelectorMock = new Mock<IBindableTypeSelector>();
            this.multiBindingCreatorMock = new Mock<IBindingCreator>();
            this.bindingGeneratorFactory = new TestBindingGeneratorFactory(
                this.bindableInterfaceSelectorMock.Object, 
                this.multiBindingCreatorMock.Object);
        }

        [Fact]
        public void NoBindingsAreCreatedForInterface()
        {
            var testee = this.bindingGeneratorFactory.CreateSelectorBindingGenerator(
                (implementationType, serviceTypes) => new Type[0]);

            var types = testee.CreateBindings(typeof(IDerivedInterface), this.kernelMock.Object);

            types.Should().BeEmpty();
        }

        [Fact]
        public void NoBindingsAreCreatedForAbstractClass()
        {
            var testee = this.bindingGeneratorFactory.CreateSelectorBindingGenerator(
                (implementationType, serviceTypes) => new Type[0]);

            var types = testee.CreateBindings(typeof(AbstractClassWithManyInterfaces), this.kernelMock.Object);

            types.Should().BeEmpty();
        }
        
        [Fact]
        public void SelectedTypesAreBound()
        {
            var type = typeof(MultipleInterfaceCrazyService);
            var expectedServices = new[] { typeof(IBar), typeof(ICrazyService) };

            var testee = this.bindingGeneratorFactory.CreateSelectorBindingGenerator(
                (implementationType, serviceTypes) => expectedServices);
            testee.CreateBindings(type, this.kernelMock.Object);

            this.multiBindingCreatorMock.Verify(mbc => mbc.CreateBindings(this.kernelMock.Object, expectedServices, type));
        }

        [Fact]
        public void AllBindableCBaseClassesAndServicesArePassedToTheSelector()
        {
            Type givenType = null;
            IEnumerable<Type> givenServiceTypes = null;
            var type = typeof(MultipleInterfaceCrazyService);
            var interfaces = new[] { typeof(IFoo), typeof(IBar), typeof(IService) };
            var baseClasses = new[] { typeof(IService<>), typeof(object) };
            
            this.bindableInterfaceSelectorMock.Setup(s => s.GetBindableInterfaces(type)).Returns(interfaces);
            this.bindableInterfaceSelectorMock.Setup(s => s.GetBindableBaseTypes(type)).Returns(baseClasses);

            var testee = this.bindingGeneratorFactory.CreateSelectorBindingGenerator(
                (implementationType, serviceTypes) =>
                {
                    givenServiceTypes = serviceTypes;
                    givenType = implementationType;
                    return new Type[0];
                });
            testee.CreateBindings(type, this.kernelMock.Object).ToList();

            givenType.Should().Be(type);
            givenServiceTypes.Should().BeEquivalentTo(interfaces.Union(baseClasses));
        }
        
        [Fact]
        public void SyntaxFromAllBindingsAreReturned()
        {
            var type = typeof(MultipleInterfaceCrazyService);
            var interfaces = new[] { typeof(IFoo), typeof(IBar<>) };
            var result = new[] { new Mock<IBindingWhenInNamedWithOrOnSyntax<object>>().Object };

            this.bindableInterfaceSelectorMock.Setup(s => s.GetBindableInterfaces(type)).Returns(interfaces);
            this.multiBindingCreatorMock.Setup(mbc => mbc.CreateBindings(this.kernelMock.Object, interfaces, type)).Returns(result);

            var testee = this.bindingGeneratorFactory.CreateSelectorBindingGenerator((timpl, tif) => interfaces);
            var syntax = testee.CreateBindings(type, this.kernelMock.Object).ToList();

            syntax.Should().BeEquivalentTo(result);
        }

        [Fact]
        public void RegexBindingGeneratorSelectsUsingRegexPattern()
        {
            var type = typeof(MultipleInterfaceCrazyService);
            var interfaces = new[]
                {
                    typeof(IFoo), 
                    typeof(IBar<>),
                    typeof(IBaseNormalInterface),
                    typeof(IBaseOfDerivedGenericInterface<>),
                    typeof(IBaseClosedGenericInterface<>)
                };
            this.bindableInterfaceSelectorMock.Setup(s => s.GetBindableInterfaces(type)).Returns(interfaces);

            var testee = this.bindingGeneratorFactory.CreateRegexBindingGenerator("IBase.*");
            testee.CreateBindings(type, this.kernelMock.Object).ToList();

            this.multiBindingCreatorMock.Verify(mbc => mbc.CreateBindings(
                this.kernelMock.Object, 
                It.Is<IEnumerable<Type>>(t => t.SequenceEqual(interfaces.Skip(2))), 
                type));
        }
    }
}
#endif