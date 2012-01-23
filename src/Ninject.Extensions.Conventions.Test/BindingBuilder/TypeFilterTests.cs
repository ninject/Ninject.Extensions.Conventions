//-------------------------------------------------------------------------------
// <copyright file="TypeFilterTests.cs" company="Ninject Project Contributors">
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

namespace Ninject.Extensions.Conventions.BindingBuilder
{
    using FluentAssertions;

    using Ninject.Extensions.Conventions.Fakes;
    using Ninject.Extensions.Conventions.Fakes.NormalClasses;

    using Xunit;

    public class TypeFilterTests
    {
        private readonly TypeFilter testee;

        public TypeFilterTests()
        {
            this.testee = new TypeFilter();
        } 

        [Fact]
        public void IsTypeInNamespaceWhenExactMatchingNamespaceReturnsTrue()
        {
            var type = typeof(DefaultConvention);

            var result = this.testee.IsTypeInNamespace(type, type.Namespace);

            result.Should().BeTrue();
        }
    
        [Fact]
        public void IsTypeInNamespaceWhenAnchestorNamespaceReturnsTrue()
        {
            var type = typeof(DefaultConvention);

            var result = this.testee.IsTypeInNamespace(type, "Ninject.Extensions.Conventions");

            result.Should().BeTrue();
        }

        [Fact]
        public void IsTypeInNamespaceWhenCompletelyOtherNamespaceReturnsFalse()
        {
            var type = typeof(DefaultConvention);

            var result = this.testee.IsTypeInNamespace(type, "System.Xml");

            result.Should().BeFalse();
        }

        [Fact]
        public void IsTypeInNamespaceWhenPartlyAnchestorNamespaceReturnsFalse()
        {
            var type = typeof(DefaultConvention);

            var result = this.testee.IsTypeInNamespace(type, "Ninject.Extensions.Conventi");

            result.Should().BeFalse();
        }
    
        [Fact]
        public void IsTypeInNamespaceWhenChildNamespaceReturnsFalse()
        {
            var type = typeof(DefaultConvention);

            var result = this.testee.IsTypeInNamespace(type, type.Namespace + ".Child");

            result.Should().BeFalse();
        }
    
        [Fact]
        public void IsTypeInheritedFromAnyWhenSelfReturnsFalse()
        {
            var type = typeof(DefaultConvention);

            var result = this.testee.IsTypeInheritedFromAny(type, new[] { type });

            result.Should().BeFalse();
        }
        
        [Fact]
        public void IsTypeInheritedFromAnyWhenBaseTypeReturnsTrue()
        {
            var type = typeof(DefaultConvention);

            var result = this.testee.IsTypeInheritedFromAny(type, new[] { type.BaseType });

            result.Should().BeTrue();
        }

        [Fact]
        public void IsTypeInheritedFromAnyWhenAnchestorReturnsTrue()
        {
            var type = typeof(DefaultConvention);

            var result = this.testee.IsTypeInheritedFromAny(type, new[] { typeof(object) });

            result.Should().BeTrue();
        }

        [Fact]
        public void IsTypeInheritedFromAnyWhenCompletelyOtherObjectReturnsFalse()
        {
            var type = typeof(DefaultConvention);

            var result = this.testee.IsTypeInheritedFromAny(type, new[] { typeof(int) });

            result.Should().BeFalse();
        }

        [Fact]
        public void IsTypeInheritedFromAnyWhenGenericTypeDefinitionIsPassedFromWhichTypeDerivesReturnsTrue()
        {
            var type = typeof(MultipleInterfaceCrazyService);

            var result = this.testee.IsTypeInheritedFromAny(type, new[] { typeof(BaseService<>) });

            result.Should().BeTrue();
        }

        [Fact]
        public void IsTypeInheritedFromAnyWhenGenericInterfaceTypeDefinitionIsPassedFromWhichTypeDerivesReturnsTrue()
        {
            var type = typeof(MultipleInterfaceCrazyService);

            var result = this.testee.IsTypeInheritedFromAny(type, new[] { typeof(IBar<>) });

            result.Should().BeTrue();
        }
        
        [Fact]
        public void HasAttributeWhenTypeHasTheSpecifiedAttributeReturnsTrue()
        {
            var type = typeof(Foo);

            var result = this.testee.HasAttribute(type, typeof(TestAttribute));

            result.Should().BeTrue();
        }

        [Fact]
        public void HasAttributeWhenTypeHasNotTheSpecifiedAttributeReturnsFalse()
        {
            var type = typeof(MultipleInterfaceCrazyService);

            var result = this.testee.HasAttribute(type, typeof(TestAttribute));

            result.Should().BeFalse();
        }
        
        [Fact]
        public void HasAttributeWhenTypeHasMatchingAttributeReturnsTrue()
        {
            var type = typeof(Foo);

            var result = this.testee.HasAttribute<TestAttribute>(type, a => a.TestValue == 1);

            result.Should().BeTrue();
        }

        [Fact]
        public void HasAttributeWhenTypeHasNoMatchingAttributeReturnsFalse()
        {
            var type = typeof(Foo);

            var result = this.testee.HasAttribute<TestAttribute>(type, a => a.TestValue != 1);

            result.Should().BeFalse();
        }
    }
}