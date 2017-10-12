//-------------------------------------------------------------------------------
// <copyright file="FactoryTests.cs" company="Ninject Project Contributors">
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

#if !SILVERLIGHT_20 && !WINDOWS_PHONE && !NETCF_35
namespace Ninject.Extensions.Conventions.IntegrationTests
{
    using System;
    using System.Linq;

    using FluentAssertions;
    using Ninject.Extensions.Conventions.Fakes;
    using Ninject.Extensions.Factory;

    using Xunit;

    public class FactoryTests : IDisposable
    {
        private readonly IKernel kernel;

        public interface IFooFactory
        {
            IFoo CreateFoo();

            IFoo CreateNamedFoo(string name);
        }

        public FactoryTests()
        {
#if !NO_ASSEMBLY_SCANNING
            this.kernel = new StandardKernel();
#else
            this.kernel = new StandardKernel(new FuncModule());
#endif
        }

        public void Dispose()
        {
            this.kernel.Dispose();
        }
        
        [Fact]
        public void DefaultFactory()
        {
            this.kernel.Bind<IFoo>().To<Foo>();
            this.kernel.Bind(
                x => x.FromThisAssembly()
                      .Select(t => t == typeof(IFooFactory))
                      .BindToFactory());

            var factory = this.kernel.Get<IFooFactory>();
            var instance = factory.CreateFoo();

            instance.Should().BeOfType<Foo>();
        }

        [Fact]
        public void CustomFactory()
        {
            this.kernel.Bind<IFoo>().To<Foo>();
            this.kernel.Bind<IFoo>().To<MyFoo>().Named("MyFoo");
            this.kernel.Bind(
                x => x.FromThisAssembly()
                      .Select(t => t == typeof(IFooFactory))
                      .BindToFactory(() => new CustomInstanceProvider()));

            var factory = this.kernel.Get<IFooFactory>();
            var instance = factory.CreateNamedFoo("MyFoo");

            instance.Should().BeOfType<MyFoo>();
        }

        public class MyFoo : IFoo { }

        private class CustomInstanceProvider : StandardInstanceProvider
        {
            protected override string GetName(System.Reflection.MethodInfo methodInfo, object[] arguments)
            {
                return (string)arguments[0];
            }

            protected override Parameters.IConstructorArgument[] GetConstructorArguments(System.Reflection.MethodInfo methodInfo, object[] arguments)
            {
                return base.GetConstructorArguments(methodInfo, arguments).Skip(1).ToArray();
            }
        }
    }
}
#endif
