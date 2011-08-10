//-------------------------------------------------------------------------------
// <copyright file="BindableTypeSelectorTest.cs" company="Ninject Project Contributors">
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
    using FluentAssertions;

    using Ninject.Extensions.Conventions.Fakes.Interfaces;
    using Ninject.Extensions.Conventions.Fakes.NormalClasses;
    using Ninject.Extensions.Conventions.Fakes.OpenGenerics;

    using Xunit;
    
    public class BindableTypeSelectorTest
    {
        private readonly BindableTypeSelector testee;

        public BindableTypeSelectorTest()
        {
            this.testee = new BindableTypeSelector();
        }
        
        [Fact]
        public void GetBindableInterfacesReturnsEmpytEnumerableForInterface()
        {
            var interfaces = this.testee.GetBindableInterfaces(typeof(IDerivedInterface));

            interfaces.Should().BeEmpty();
        }
    
        [Fact]
        public void GetBindableInterfacesReturnsEmpytEnumerableForAbstractClass()
        {
            var interfaces = this.testee.GetBindableInterfaces(typeof(AbstractClassWithManyInterfaces));

            interfaces.Should().BeEmpty();
        }

        [Fact]
        public void GetBindableInterfacesReturnsAllInterfacesForNormalClass()
        {
            var interfaces = this.testee.GetBindableInterfaces(typeof(ClassWithManyInterfaces));

            interfaces.Should().BeEquivalentTo(
                new[]
                    {
                        typeof(INormalInterface), 
                        typeof(IClosedGenericInterface<int>), 
                        typeof(IBaseNormalInterface), 
                        typeof(IBaseOpenGenericInterface1<int>),
                        typeof(IBaseOpenGenericInterface2<int>),
                        typeof(IBaseOpenGenericInterface<int, int>),
                        typeof(IBaseClosedGenericInterface<int>),
                        typeof(IDerivedInterface),
                        typeof(IBaseOfDerivedInterface),
                        typeof(IDerivedGenericInterface<int>),
                        typeof(IBaseOfDerivedGenericInterface<int>),
                    });
        }

        [Fact]
        public void GetBindableInterfacesReturnsAllOpenGenericInterfacesForOpenGenericClass()
        {
            var interfaces = this.testee.GetBindableInterfaces(typeof(OpenGenericClassWithManyInterfaces<>));

            interfaces.Should().BeEquivalentTo(
                new[]
                    {
                        typeof(IOpenGenericInterface<>), 
                        typeof(IBaseOpenGenericInterface1<>),
                        typeof(IDerivedGenericInterface<>),
                        typeof(IBaseOfDerivedGenericInterface<>),
                    });
        }

        [Fact]
        public void GetBindableInterfacesReturnsAllOpenGenericInterfacesForOpenGenericClass2()
        {
            var interfaces = this.testee.GetBindableInterfaces(typeof(GenericBaseClassWithManyInterfaces<,>));

            interfaces.Should().BeEquivalentTo(
                new[]
                    {
                        typeof(IBaseOpenGenericInterface<,>),
                    });
        }
        
        [Fact]
        public void GetBindableBaseTypesReturnsEmpytEnumerableForInterface()
        {
            var interfaces = this.testee.GetBindableBaseTypes(typeof(IDerivedInterface));

            interfaces.Should().BeEmpty();
        }

        [Fact]
        public void GetBindableBaseTypesReturnsEmpytEnumerableForAbstractClass()
        {
            var interfaces = this.testee.GetBindableBaseTypes(typeof(AbstractClassWithManyInterfaces));

            interfaces.Should().BeEmpty();
        }

        [Fact]
        public void GetBindableBaseTypesReturnsAllBaseClassesForNormalClass()
        {
            var interfaces = this.testee.GetBindableBaseTypes(typeof(ClassWithManyInterfaces));

            interfaces.Should().BeEquivalentTo(
                new[]
                    {
                        typeof(GenericBaseClassWithManyInterfaces<int, int>), 
                        typeof(object),
                    });
        }

        [Fact]
        public void GetBindableBaseTypesReturnsAllOpenGenericForOpenGenericClass()
        {
            var interfaces = this.testee.GetBindableBaseTypes(typeof(OpenGenericClassWithManyInterfaces<>));

            interfaces.Should().BeEquivalentTo(
                new[]
                    {
                        typeof(GenericBaseClassWithManyInterfaces<>), 
                    });
        }       
    }
}
#endif