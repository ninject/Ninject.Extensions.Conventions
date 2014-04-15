//-------------------------------------------------------------------------------
// <copyright file="ConventionSyntaxBindingTests.cs" company="Ninject Project Contributors">
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

#if !NO_MOQ
namespace Ninject.Extensions.Conventions.BindingBuilder
{
    using System;
    using System.Text.RegularExpressions;
    using Moq;
    using Ninject.Extensions.Conventions.BindingGenerators;
    using Ninject.Extensions.Conventions.Syntax;

    using Xunit;

    public class ConventionSyntaxBindingTests
    {
        private readonly Mock<IConventionBindingBuilder> conventionBindingBuilderMock;

        private readonly ConventionSyntax testee;

        private readonly Mock<IBindingGeneratorFactory> bindingGeneratorFactoryMock;

        public ConventionSyntaxBindingTests()
        {
            this.conventionBindingBuilderMock = new Mock<IConventionBindingBuilder>();
            this.bindingGeneratorFactoryMock = new Mock<IBindingGeneratorFactory>();
#if !NO_ASSEMBLY_SCANNING
            this.testee = new ConventionSyntax(this.conventionBindingBuilderMock.Object, null, null, this.bindingGeneratorFactoryMock.Object);
#else
            this.testee = new ConventionSyntax(this.conventionBindingBuilderMock.Object, null, this.bindingGeneratorFactoryMock.Object);
#endif
            }       
 
        [Fact]
        public void BindWithGeneric()
        {
            this.testee.BindWith<BaseBindingGenerator>();

            this.conventionBindingBuilderMock.Verify(b => b.BindWith(It.Is<IBindingGenerator>(g => g is BaseBindingGenerator)));
        }

        [Fact]
        public void BindWith()
        {
            var generator = new Mock<IBindingGenerator>().Object;

            this.testee.BindWith(generator);

            this.conventionBindingBuilderMock.Verify(b => b.BindWith(generator));
        }

        [Fact]
        public void BindGeneric()
        {
            Type serviceType = typeof(string);
            var generator = new Mock<IBindingGenerator>().Object;
            this.bindingGeneratorFactoryMock
                .Setup(g => g.CreateSpecificTypesBindingGenerator(serviceType))
                .Returns(generator);

            this.testee.Bind<string>();

            this.conventionBindingBuilderMock.Verify(b => b.BindWith(generator));
        }

        [Fact]
        public void Bind()
        {
            Type[] serviceTypes = { typeof(string), typeof(object) };
            var generator = new Mock<IBindingGenerator>().Object;
            this.bindingGeneratorFactoryMock
                .Setup(g => g.CreateSpecificTypesBindingGenerator(serviceTypes))
                .Returns(generator);

            this.testee.Bind(serviceTypes);

            this.conventionBindingBuilderMock.Verify(b => b.BindWith(generator));
        }

        [Fact]
        public void BindToAllInterfaces()
        {
            var generator = new Mock<IBindingGenerator>().Object;
            this.bindingGeneratorFactoryMock.Setup(g => g.CreateAllInterfacesBindingGenerator()).Returns(generator);

            this.testee.BindAllInterfaces();

            this.conventionBindingBuilderMock.Verify(b => b.BindWith(generator));
        }

        [Fact]
        public void BindToAllBaseClasses()
        {
            var generator = new Mock<IBindingGenerator>().Object;
            this.bindingGeneratorFactoryMock.Setup(g => g.CreateAllBaseClassesBindingGenerator()).Returns(generator);

            this.testee.BindAllBaseClasses();

            this.conventionBindingBuilderMock.Verify(b => b.BindWith(generator));
        }

        [Fact]
        public void BindToBase()
        {
            var generator = new Mock<IBindingGenerator>().Object;
            this.bindingGeneratorFactoryMock.Setup(g => g.CreateBaseBindingGenerator()).Returns(generator);

            this.testee.BindBase();

            this.conventionBindingBuilderMock.Verify(b => b.BindWith(generator));
        }

        [Fact]
        public void BindToDefaultInterface()
        {
            var generator = new Mock<IBindingGenerator>().Object;
            this.bindingGeneratorFactoryMock.Setup(g => g.CreateDefaultInterfaceBindingGenerator()).Returns(generator);

            this.testee.BindDefaultInterface();

            this.conventionBindingBuilderMock.Verify(b => b.BindWith(generator));
        }

        [Fact]
        public void BindToDefaultInterfaces()
        {
            var generator = new Mock<IBindingGenerator>().Object;
            this.bindingGeneratorFactoryMock.Setup(g => g.CreateDefaultInterfacesBindingGenerator()).Returns(generator);

            this.testee.BindDefaultInterfaces();

            this.conventionBindingBuilderMock.Verify(b => b.BindWith(generator));
        }
        
        [Fact]
        public void BindToSelf()
        {
            var generator = new Mock<IBindingGenerator>().Object;
            this.bindingGeneratorFactoryMock.Setup(g => g.CreateSelfBindingGenerator()).Returns(generator);

            this.testee.BindToSelf();

            this.conventionBindingBuilderMock.Verify(b => b.BindWith(generator));
        }

        [Fact]
        public void BindToSingleInterface()
        {
            var generator = new Mock<IBindingGenerator>().Object;
            this.bindingGeneratorFactoryMock.Setup(g => g.CreateSingleInterfaceBindingGenerator()).Returns(generator);

            this.testee.BindSingleInterface();

            this.conventionBindingBuilderMock.Verify(b => b.BindWith(generator));
        }

        [Fact]
        public void BindToSelection()
        {
            ServiceSelector selector = (t1, t2) => t2;
            var generator = new Mock<IBindingGenerator>().Object;
            this.bindingGeneratorFactoryMock.Setup(g => g.CreateSelectorBindingGenerator(selector)).Returns(generator);

            this.testee.BindSelection(selector);

            this.conventionBindingBuilderMock.Verify(b => b.BindWith(generator));
        }

        [Fact]
        public void BindToRegex()
        {
            const string Pattern = "ThePattern";
            var generator = new Mock<IBindingGenerator>().Object;
            this.bindingGeneratorFactoryMock.Setup(g => g.CreateRegexBindingGenerator(Pattern)).Returns(generator);

            this.testee.BindUsingRegex(Pattern);

            this.conventionBindingBuilderMock.Verify(b => b.BindWith(generator));
        }

        [Fact]
        public void BindToRegexWithOptions()
        {
            const string Pattern = "ThePattern";
            const RegexOptions Options = RegexOptions.Multiline;
            var generator = new Mock<IBindingGenerator>().Object;
            this.bindingGeneratorFactoryMock.Setup(g => g.CreateRegexBindingGenerator(Pattern, Options)).Returns(generator);

            this.testee.BindUsingRegex(Pattern, Options);

            this.conventionBindingBuilderMock.Verify(b => b.BindWith(generator));
        }

        [Fact]
        public void Configure()
        {
            ConfigurationAction configurationAction = s => s.InThreadScope();
            
            this.testee.Configure(configurationAction);

            this.conventionBindingBuilderMock.Verify(b => b.Configure(configurationAction));
        }

        [Fact]
        public void ConfigureWithService()
        {
            ConfigurationActionWithService configurationAction = (c, s) => c.InThreadScope();

            this.testee.Configure(configurationAction);

            this.conventionBindingBuilderMock.Verify(b => b.Configure(configurationAction));
        }

        [Fact]
        public void ConfigureFor()
        {
            ConfigurationAction configurationAction = s => s.InThreadScope();

            this.testee.ConfigureFor<int>(configurationAction);

            this.conventionBindingBuilderMock.Verify(b => b.ConfigureFor<int>(configurationAction));
        }

        [Fact]
        public void ConfigureForWithService()
        {
            ConfigurationActionWithService configurationAction = (c, s) => c.InThreadScope();

            this.testee.ConfigureFor<string>(configurationAction);

            this.conventionBindingBuilderMock.Verify(b => b.ConfigureFor<string>(configurationAction));
        }
    }
}
#endif