//-------------------------------------------------------------------------------
// <copyright file="ModuleLoadingTest.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2012 Ninject Project Contributors
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

namespace Ninject.Extensions.Conventions.IntegrationTests
{
    using System.Reflection;

    using FluentAssertions;

    using Ninject.Extensions.Conventions.Fakes.Interfaces;
    using Ninject.Extensions.Conventions.Fakes.NormalClasses;
    using Ninject.Modules;

    using Xunit;

#if !SILVERLIGHT
    public class ModuleLoadingTest
    {
        [Fact]
        public void ConventionsCanBeUsedInModules()
        {
            using (IKernel kernel = new StandardKernel())
            {
                kernel.Load<TestModule>();
                var instance = kernel.Get<IDefaultConvention>();

                instance.Should().NotBeNull();
                instance.Should().BeOfType<DefaultConvention>();
            }
        }
        
        private class TestModule : NinjectModule
        {
            public override void Load()
            {
                this.Bind(
                    x => x.From(Assembly.GetExecutingAssembly())
                          .SelectAllTypes()
                          .BindDefaultInterface());
            }
        }
    }
#endif
}