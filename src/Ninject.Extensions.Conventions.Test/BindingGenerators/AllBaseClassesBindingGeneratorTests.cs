//-------------------------------------------------------------------------------
// <copyright file="AbstractClassWithManyInterfaces.cs" company="Ninject Project Contributors">
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

#if !SILVERLIGHT_30 && !SILVERLIGHT_20 && !NO_MOQ && !NO_GENERIC_MOQ
namespace Ninject.Extensions.Conventions.BindingGenerators
{
    using FluentAssertions;
    using Ninject.Extensions.Conventions.BindingBuilder;
    using Ninject.Extensions.Conventions.Fakes;
    using Ninject.Extensions.Conventions.Fakes.AbstractInheritanceTree;
    using Ninject.Extensions.Conventions.Fakes.Interfaces;
    using Ninject.Extensions.Conventions.Fakes.NormalClasses;
    using Xunit;

    public class AllBaseClassesBindingGeneratorTests
    {
        private readonly KernelMock kernelMock;
        private readonly IBindingGenerator testee;

        public AllBaseClassesBindingGeneratorTests()
        {
            this.kernelMock = new KernelMock();
            var bindingGeneratorFactory = new BindingGeneratorFactory(null);
            this.testee = bindingGeneratorFactory.CreateAllBaseClassesBindingGenerator();
        }

        [Fact]
        public void NoBindingsAreCreatedForInterface()
        {
            var types = this.testee.CreateBindings(typeof(IDerivedInterface), this.kernelMock.Object);

            types.Should().BeEmpty();
        }

        [Fact]
        public void NoBindingsAreCreatedForAbstractClass()
        {
            var types = this.testee.CreateBindings(typeof(AbstractClassWithManyInterfaces), this.kernelMock.Object);

            types.Should().BeEmpty();
        }

        [Fact]
        public void BindingForBaseTypeIsCreated()
        {
            var type = typeof(MultipleInterfaceCrazyService);

            this.testee.CreateBindings(type, this.kernelMock.Object);

            this.kernelMock.VerifyAllBindingsCreated(new[] { typeof(BaseService<int>) }, type);
        }

        [Fact]
        public void SyntaxFormAllBindingsAreReturned()
        {
            var type = typeof(MultipleInterfaceCrazyService);

            var syntax = this.testee.CreateBindings(type, this.kernelMock.Object);

            syntax.Should().BeEquivalentTo(this.kernelMock.ReturnedSyntax);
        }        

        [Fact]
        public void BindingForAnyInheritorOfAbstractTypeIsCreated()
        {
            var type = typeof(DerivedAbstractClassImpl);

            this.testee.CreateBindings(type, this.kernelMock.Object);

            this.kernelMock.VerifyAllBindingsCreated(new[] { typeof(BaseAbstractClass), typeof(DerivedAbstractClass) }, type);
        }

        [Fact]
        public void BindingForAnyInheritorOfTypeIsCreated()
        {
            var type = typeof(LeafClass);

            this.testee.CreateBindings(type, this.kernelMock.Object);
            
            this.kernelMock.VerifyAllBindingsCreated(new[] { typeof(BaseAbstractClass), typeof(DerivedAbstractClass), typeof(DerivedAbstractClassImpl) }, type);
        }
    }
}
#endif