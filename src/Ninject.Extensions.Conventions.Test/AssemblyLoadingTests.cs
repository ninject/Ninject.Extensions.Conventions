namespace Ninject.Extensions.Conventions
{
    using System.Reflection;
    using Ninject.Extensions.Conventions.Fakes;
#if SILVERLIGHT
#if SILVERLIGHT_MSTEST
    using MsTest.Should;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Fact = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
#else
    using UnitDriven;
    using UnitDriven.Should;
    using Fact = UnitDriven.TestMethodAttribute;
#endif
#else
    using Ninject.Tests.MSTestAttributes;
    using Xunit;
    using Xunit.Should;
#endif

    [TestClass]
    public class AssemblyLoadingTests
    {
        [Fact]
        public void SpecifyingBindingGeneratorTypeResolvesCorrectly()
        {
            using (IKernel kernel = new StandardKernel())
            {
                var scanner = new AssemblyScanner();
                scanner.From(Assembly.GetExecutingAssembly());
                scanner.BindWith<DefaultBindingGenerator>();
                kernel.Scan(scanner);
                var instance = kernel.Get<IDefaultConvention>();

                instance.ShouldNotBeNull();
                instance.ShouldBeInstanceOf<DefaultConvention>();
            }
        }

        [Fact]
        public void UsingDefaultConventionsResolvesCorrectly()
        {
            using (IKernel kernel = new StandardKernel())
            {
                var scanner = new AssemblyScanner();
                scanner.From(Assembly.GetExecutingAssembly());
                scanner.BindWithDefaultConventions();
                kernel.Scan(scanner);
                var instance = kernel.Get<IDefaultConvention>();

                instance.ShouldNotBeNull();
                instance.ShouldBeInstanceOf<DefaultConvention>();
            }
        }

        [Fact]
        public void TestBindingGeneratorInLambaSyntax()
        {
            using (IKernel kernel = new StandardKernel())
            {
                kernel.Scan(x =>
                             {
                                 x.From(Assembly.GetExecutingAssembly());
                                 x.BindWith<DefaultBindingGenerator>();
                             });
                var instance = kernel.Get<IDefaultConvention>();

                instance.ShouldNotBeNull();
                instance.ShouldBeInstanceOf<DefaultConvention>();
            }
        }

#if !SILVERLIGHT
        [Fact]
        public void LoadAssemblyByFullQualifiedName()
        {
            using (IKernel kernel = new StandardKernel())
            {
                kernel.Scan(x =>
                {
                    x.From("TestPlugin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
                    x.BindWith<DefaultBindingGenerator>();
                });
                
                var instance = kernel.Get<IDefaultConvention>();

                instance.ShouldNotBeNull();
                instance.Name.ShouldBe("DefaultConventionFromPlugin");
            }
        }
#endif
    }
}