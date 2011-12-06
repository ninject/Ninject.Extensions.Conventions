//-------------------------------------------------------------------------------
// <copyright file="AllInterfacesBindingGeneratorTests.cs" company="Ninject Project Contributors">
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

#if !SILVERLIGHT_30 && !SILVERLIGHT_20 && !NO_MOQ
namespace Ninject.Extensions.Conventions.BindingGenerators
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Moq;

    using Ninject.Extensions.Conventions.BindingBuilder;
    using Ninject.Extensions.Conventions.Fakes;
    using Ninject.Planning.Bindings;
    using Ninject.Syntax;

    using Xunit;

    public class AllInterfacesBindingGeneratorTests
    {
        private readonly IBindingGenerator testee;
        private readonly IKernel kernel;
        private readonly Mock<IBindableTypeSelector> bindableInterfaceSelectorMock;
        private readonly Mock<IBindingCreator> multiBindingCreatorMock;

        public AllInterfacesBindingGeneratorTests()
        {
            this.kernel = new Mock<IKernel>().Object;
            this.bindableInterfaceSelectorMock = new Mock<IBindableTypeSelector>();
            this.multiBindingCreatorMock = new Mock<IBindingCreator>();
            this.testee = new AllInterfacesBindingGenerator(this.bindableInterfaceSelectorMock.Object, this.multiBindingCreatorMock.Object);
        }

        [Fact]
        public void AllInterfacesAreBound()
        {
            var type = typeof(MultipleInterfaceCrazyService);
            var interfaces = new[] { typeof(IFoo), typeof(IBar<>) };
            this.bindableInterfaceSelectorMock.Setup(s => s.GetBindableInterfaces(type)).Returns(interfaces);

            this.testee.CreateBindings(type, this.kernel);

            this.multiBindingCreatorMock.Verify(mbc => mbc.CreateBindings(this.kernel, interfaces, type));
        }
        
        [Fact]
        public void BindingSyntaxReturnedByMultiBindingCreatorIsReturned()
        {
            var type = typeof(MultipleInterfaceCrazyService);
            var interfaces = new[] { typeof(IFoo), typeof(IBar<>) };
            var result = new[] { new Mock<IBindingWhenInNamedWithOrOnSyntax<object>>().Object };

            this.bindableInterfaceSelectorMock.Setup(s => s.GetBindableInterfaces(type)).Returns(interfaces);
            this.multiBindingCreatorMock.Setup(mbc => mbc.CreateBindings(this.kernel, interfaces, type)).Returns(result);

            var syntax = this.testee.CreateBindings(type, this.kernel).ToList();

            syntax.Should().BeEquivalentTo(result);
        }
    }
}
#endif