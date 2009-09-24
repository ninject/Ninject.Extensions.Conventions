#region Using Directives

using System.Reflection;
using Ninject.Extensions.Conventions.Tests.Fakes;
using Xunit;

#endregion

namespace Ninject.Extensions.Conventions.Tests
{
    public class AssemblyLoadingTests
    {
        [Fact]
        public void SpecifyingBindingGeneratorTypeResolvesCorrectly()
        {
            using ( IKernel kernel = new StandardKernel() )
            {
                var scanner = new AssemblyScanner();
                scanner.Assembly( Assembly.GetExecutingAssembly() );
                scanner.Using<DefaultBindingGenerator>();
                kernel.Scan( scanner );
                var instance = kernel.Get<IDefaultConvention>();
                Assert.NotNull( instance );
                Assert.Equal( typeof (DefaultConvention), instance.GetType() );
            }
        }

        [Fact]
        public void UsingDefaultConventionsResolvesCorrectly()
        {
            using ( IKernel kernel = new StandardKernel() )
            {
                var scanner = new AssemblyScanner();
                scanner.Assembly( Assembly.GetExecutingAssembly() );
                scanner.UsingDefaultConventions();
                kernel.Scan( scanner );
                var instance = kernel.Get<IDefaultConvention>();
                Assert.NotNull( instance );
                Assert.Equal( typeof (DefaultConvention), instance.GetType() );
            }
        }

        [Fact]
        public void TestBindingGeneratorInLambaSyntax()
        {
            using ( IKernel kernel = new StandardKernel() )
            {
                kernel.Scan( x =>
                             {
                                 x.Assembly( Assembly.GetExecutingAssembly() );
                                 x.Using<DefaultBindingGenerator>();
                             }
                    );
                var instance = kernel.Get<IDefaultConvention>();
                Assert.NotNull( instance );
                Assert.Equal( typeof (DefaultConvention), instance.GetType() );
            }
        }
    }
}