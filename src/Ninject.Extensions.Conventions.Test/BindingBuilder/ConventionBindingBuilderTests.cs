//-------------------------------------------------------------------------------
// <copyright file="ConventionBindingBuilderTests.cs" company="Ninject Project Contributors">
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
    using System.Reflection;

    using Moq;

    using Ninject.Extensions.Conventions.BindingGenerators;
    using Ninject.Extensions.Conventions.Fakes.NormalClasses;
    using Ninject.Syntax;

    using Xunit;

    public class ConventionBindingBuilderTests
    {
        private readonly IBindingRoot bindingRoot;

        private readonly ConventionBindingBuilder testee;

        private readonly Mock<ITypeSelector> typeSelectorMock;

        public ConventionBindingBuilderTests()
        {
            this.bindingRoot = new Mock<IBindingRoot>().Object;
            this.typeSelectorMock = new Mock<ITypeSelector>();
            this.testee = new ConventionBindingBuilder(this.bindingRoot, this.typeSelectorMock.Object);
        }

        [Fact]
        public void SelectAllTypesFromPassesAssembliesToTypeFilterGetTypesToReceiveAllExportedTypes()
        {
            var assemblies = new[] { Assembly.GetExecutingAssembly(), Assembly.GetCallingAssembly() };

            this.testee.SelectAllTypesFrom(assemblies);

            this.typeSelectorMock.Verify(tf => tf.GetExportedTypes(assemblies));
        }

#if !NO_SKIP_VISIBILITY
        [Fact]
        public void IncludingNonePublicTypesPassesAssembliesToTypeFilterGetTypesToReceiveAllTypes()
        {
            var assemblies = new[] { Assembly.GetExecutingAssembly(), Assembly.GetCallingAssembly() };

            this.testee.SelectAllTypesFrom(assemblies);
            this.testee.IncludingNonePublicTypes();

            this.typeSelectorMock.Verify(tf => tf.GetAllTypes(assemblies));
        }
#endif
        
        [Fact]
        public void AllTypesReturnedByTypeFilterArePassedToTheGeneratorOnBindWith()
        {
            var types = new[] { typeof(int), typeof(object) };
            var generatorMock = new Mock<IBindingGenerator>();

            this.SetupTypeFilterGetTypes(types);

            this.testee.SelectAllTypesFrom(new Assembly[0]);
            this.testee.BindWith(generatorMock.Object);

            this.VerifyAllBindingsCreated(types, generatorMock);
        }

        [Fact]
        public void WhereFiltersNoneMatchingTypes()
        {
            var expectedTypes = new[] { typeof(int), typeof(object) };
            var types = expectedTypes.Union(new[] { typeof(DefaultConvention) });
            var generatorMock = new Mock<IBindingGenerator>();

            this.SetupTypeFilterGetTypes(types);

            this.testee.SelectAllTypesFrom(new Assembly[0]);
            this.testee.Where(t => t.FullName.StartsWith("System."));
            this.testee.BindWith(generatorMock.Object);

            this.VerifyAllBindingsCreated(expectedTypes, generatorMock);
        }

        [Fact]
        public void IncludingAddsTheGivenType()
        {
            var types = new[] { typeof(int), typeof(object) };
            var expectedTypes = types.Union(new[] { typeof(DefaultConvention) });
            var generatorMock = new Mock<IBindingGenerator>();

            this.SetupTypeFilterGetTypes(types);

            this.testee.SelectAllTypesFrom(new Assembly[0]);
            this.testee.Including(new[] { typeof(DefaultConvention) });
            this.testee.BindWith(generatorMock.Object);

            this.VerifyAllBindingsCreated(expectedTypes, generatorMock);
        }

        [Fact]
        public void ExcludingRemovesTheGivenType()
        {
            var expectedTypes = new[] { typeof(int), typeof(object) };
            var types = expectedTypes.Union(new[] { typeof(DefaultConvention) });
            var generatorMock = new Mock<IBindingGenerator>();

            this.SetupTypeFilterGetTypes(types);

            this.testee.SelectAllTypesFrom(new Assembly[0]);
            this.testee.Excluding(new[] { typeof(DefaultConvention) });
            this.testee.BindWith(generatorMock.Object);

            this.VerifyAllBindingsCreated(expectedTypes, generatorMock);
        }

        [Fact]
        public void TypesArePassedDistinctToTheGenerator()
        {
            var expectedTypes = new[] { typeof(int), typeof(object) };
            var types = expectedTypes.Union(expectedTypes);
            var generatorMock = new Mock<IBindingGenerator>();

            this.SetupTypeFilterGetTypes(types);

            this.testee.SelectAllTypesFrom(new Assembly[0]);
            this.testee.BindWith(generatorMock.Object);

            this.VerifyAllBindingsCreated(expectedTypes, generatorMock);
        }

        [Fact]
        public void MultipleIncludesAndExcludesCanBeDone()
        {
            var expectedTypes = new[] { typeof(int), typeof(object), typeof(double) };
            var types = new[] { typeof(object), typeof(float) };
            var generatorMock = new Mock<IBindingGenerator>();

            this.SetupTypeFilterGetTypes(types);

            this.testee.SelectAllTypesFrom(new Assembly[0]);
            this.testee.Including(new[] { typeof(DefaultConvention) });
            this.testee.Excluding(new[] { typeof(float) });
            this.testee.Including(new[] { typeof(int), typeof(double) });
            this.testee.Excluding(new[] { typeof(DefaultConvention), typeof(int) });
            this.testee.Including(new[] { typeof(int) });
            this.testee.BindWith(generatorMock.Object);

            this.VerifyAllBindingsCreated(expectedTypes, generatorMock);
        }
        
        [Fact]
        public void AdditionalTypesCanBeSelectedFromOtherAssembliesUsingFromSelectAgain()
        {
            var assemblies1 = new[] { Assembly.GetExecutingAssembly() };
            var assemblies2 = new[] { Assembly.GetCallingAssembly() };
            var types1 = new[] { typeof(object), typeof(float) };
            var types2 = new[] { typeof(double), typeof(int) };
            var generatorMock = new Mock<IBindingGenerator>();

            this.SetupTypeFilterGetTypes(assemblies1, types1);
            this.SetupTypeFilterGetTypes(assemblies2, types2);

            this.testee.SelectAllTypesFrom(assemblies1);
            this.testee.SelectAllTypesFrom(assemblies2);
            this.testee.BindWith(generatorMock.Object);

            this.VerifyAllBindingsCreated(types1.Union(types2), generatorMock);
        }

        [Fact]
        public void Configure()
        {
            var types = new[] { typeof(int), typeof(object) };
            var generatorMock = new Mock<IBindingGenerator>();
            var intBindingMocks = new[] { CreateBindingMock(), CreateBindingMock() };
            var objectBindingMocks = new[] { CreateBindingMock(), CreateBindingMock() };

            this.SetupTypeFilterGetTypes(types);
            generatorMock.Setup(g => g.CreateBindings(typeof(int), this.bindingRoot)).Returns(intBindingMocks.Select(b => b.Object));
            generatorMock.Setup(g => g.CreateBindings(typeof(object), this.bindingRoot)).Returns(objectBindingMocks.Select(b => b.Object));

            this.testee.SelectAllTypesFrom(new Assembly[0]);
            this.testee.BindWith(generatorMock.Object);
            this.testee.Configure(c => c.InSingletonScope());

            foreach (var bindingMock in intBindingMocks.Union(objectBindingMocks))
            {
                bindingMock.Verify(b => b.InSingletonScope());
            }
        }

        private static Mock<IBindingWhenInNamedWithOrOnSyntax<object>> CreateBindingMock()
        {
            return new Mock<IBindingWhenInNamedWithOrOnSyntax<object>>();
        }

        private void SetupTypeFilterGetTypes(IEnumerable<Type> returnedTypes)
        {
            this.typeSelectorMock.Setup(tf => tf.GetExportedTypes(It.IsAny<IEnumerable<Assembly>>())).Returns(returnedTypes);
        }

        private void SetupTypeFilterGetTypes(IEnumerable<Assembly> expectedAssemblies, IEnumerable<Type> returnedTypes)
        {
            this.typeSelectorMock.Setup(tf => tf.GetExportedTypes(expectedAssemblies)).Returns(returnedTypes);
        }
        
        private void VerifyAllBindingsCreated(IEnumerable<Type> types, Mock<IBindingGenerator> generatorMock)
        {
            foreach (var type in types)
            {
                generatorMock.Verify(g => g.CreateBindings(type, this.bindingRoot), Times.Once());
            }
 
            generatorMock.Verify(g => g.CreateBindings(It.Is<Type>(t => !types.Contains(t)), this.bindingRoot), Times.Never());
        }
    }
}
#endif