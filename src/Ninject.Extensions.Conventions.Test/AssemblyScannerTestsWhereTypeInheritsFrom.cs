namespace Ninject.Extensions.Conventions
{
    using System.Linq;
    using Ninject.Extensions.Conventions.Fakes;
    using Ninject.Infrastructure;
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
    public class AssemblyScannerTestsWhereTypeInheritsFrom
    {
        [Fact]
        public void DefaultConventionsApply()
        {
            using (IKernel kernel = new StandardKernel())
            {
                AssemblyScanner scanner = new AssemblyScanner();
                scanner.From(typeof(DefaultView).Assembly);
                scanner.WhereTypeInheritsFrom<IDefaultView>();
                scanner.BindWithDefaultConventions();
                scanner.Process(kernel);
                kernel.GetBindings(typeof(IDefaultView)).Count().ShouldBe(1);
                kernel.Get<IDefaultView>().ShouldBeInstanceOf<DefaultView>();
            }
        }
    }
}
