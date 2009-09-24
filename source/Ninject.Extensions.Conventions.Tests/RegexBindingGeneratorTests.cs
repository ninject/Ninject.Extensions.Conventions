#region Using Directives

using System.Linq;
using Ninject.Extensions.Conventions.Tests.Fakes;
using Xunit;

#endregion

namespace Ninject.Extensions.Conventions.Tests
{
    public class RegexBindingGeneratorTests
    {
        [Fact]
        public void CanResolveMatchInMiddleOfInterface()
        {
            var regexBindingGenerator = new RegexBindingGenerator( "(I)(?<name>.+)(View)" );
            using ( IKernel kernel = new StandardKernel() )
            {
                regexBindingGenerator.Process( typeof (DefaultView), kernel );
                Assert.Equal( kernel.GetBindings( typeof (IDefaultView) ).Count(), 1 );
                Assert.IsType( typeof (DefaultView), kernel.Get<IDefaultView>() );
            }
        }

        [Fact]
        public void CanResolveMatchAtEndOfInterface()
        {
            var regexBindingGenerator = new RegexBindingGenerator( "(I)(?<name>.+)" );
            using ( IKernel kernel = new StandardKernel() )
            {
                regexBindingGenerator.Process( typeof (DefaultView), kernel );
                Assert.Equal( kernel.GetBindings( typeof (IDefaultView) ).Count(), 1 );
                Assert.IsType( typeof (DefaultView), kernel.Get<IDefaultView>() );
                Assert.Equal( kernel.GetBindings( typeof (IDefaultConvention) ).Count(), 1 );
                Assert.IsType( typeof (DefaultView), kernel.Get<IDefaultConvention>() );
            }
        }

        [Fact]
        public void CanResolveDetailedMatchAtEndOfInterface()
        {
            var regexBindingGenerator = new RegexBindingGenerator( "(IDefault)(?<name>.+)" );
            using ( IKernel kernel = new StandardKernel() )
            {
                regexBindingGenerator.Process( typeof (DefaultView), kernel );
                Assert.Equal( kernel.GetBindings( typeof (IDefaultView) ).Count(), 1 );
                Assert.IsType( typeof (DefaultView), kernel.Get<IDefaultView>() );
                Assert.Equal( kernel.GetBindings( typeof (IDefaultConvention) ).Count(), 1 );
                Assert.IsType( typeof (DefaultView), kernel.Get<IDefaultConvention>() );
            }
        }
    }
}