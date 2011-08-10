//-------------------------------------------------------------------------------
// <copyright file="ConventionSyntaxFilteringTests.cs" company="Ninject Project Contributors">
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

#if !NO_MOQ
namespace Ninject.Extensions.Conventions.BindingBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Moq;
    using Xunit;

    public class ConventionSyntaxFilteringTests
    {
        private readonly Mock<IConventionBindingBuilder> conventionBindingBuilderMock;

        private readonly ConventionSyntax testee;

        public ConventionSyntaxFilteringTests()
        {
            this.conventionBindingBuilderMock = new Mock<IConventionBindingBuilder>();
#if !NO_ASSEMBLY_SCANNING
            this.testee = new ConventionSyntax(this.conventionBindingBuilderMock.Object, null, null, null);
#else
            this.testee = new ConventionSyntax(this.conventionBindingBuilderMock.Object, null, null);
#endif            
        }   
     
        [Fact]
        public void Where()
        {
            Func<Type, bool> filter = t => true;

            this.testee.Where(filter);

            this.conventionBindingBuilderMock.Verify(b => b.Where(filter));
        }

        [Fact]
        public void IncludingWithParams()
        {
            var types = new[] { typeof(object) };

            this.testee.Including(types);

            this.conventionBindingBuilderMock.Verify(b => b.Including(types));
        }

        [Fact]
        public void IncludingWithEnumerable()
        {
            var types = new[] { typeof(object) };

            this.testee.Including(types.AsEnumerable());

            this.conventionBindingBuilderMock.Verify(b => b.Including(types));
        }

        [Fact]
        public void IncludingWithSingleType()
        {
            var type = typeof(object);

            this.testee.Including(type);

            this.conventionBindingBuilderMock.Verify(b => b.Including(IsMatchingSequence(type)));
        }

        [Fact]
        public void IncludingWithGenericType()
        {
            var type = typeof(object);

            this.testee.Including<object>();

            this.conventionBindingBuilderMock.Verify(b => b.Including(IsMatchingSequence(type)));
        }

        [Fact]
        public void ExcludingWithParams()
        {
            var types = new[] { typeof(object) };

            this.testee.Excluding(types);

            this.conventionBindingBuilderMock.Verify(b => b.Excluding(types));
        }

        [Fact]
        public void ExcludingWithEnumerable()
        {
            var types = new[] { typeof(object) };

            this.testee.Excluding(types.AsEnumerable());

            this.conventionBindingBuilderMock.Verify(b => b.Excluding(types));
        }

        [Fact]
        public void ExcludingWithSingleType()
        {
            var type = typeof(object);

            this.testee.Excluding(type);

            this.conventionBindingBuilderMock.Verify(b => b.Excluding(IsMatchingSequence(type)));
        }

        [Fact]
        public void ExcludingWithGenericType()
        {
            var type = typeof(object);

            this.testee.Excluding<object>();

            this.conventionBindingBuilderMock.Verify(b => b.Excluding(IsMatchingSequence(type)));
        }
        
        private static IEnumerable<T> IsMatchingSequence<T>(params T[] values)
        {
#if !SILVERLIGHT_30 && !SILVERLIGHT_20
            return Match.Create<IEnumerable<T>>(value => value.SequenceEqual(values));
#else            
            return Match<IEnumerable<T>>.Create(value => value.SequenceEqual(values));
#endif
        }
    }
}
#endif