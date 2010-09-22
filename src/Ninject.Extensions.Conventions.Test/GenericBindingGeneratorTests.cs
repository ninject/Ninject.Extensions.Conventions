namespace Ninject.Extensions.Conventions.Tests
{
    using Ninject.Extensions.Conventions.Tests.Fakes;
#if SILVERLIGHT
#if SILVERLIGHT_MSTEST
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Assert = Ninject.SilverlightTests.AssertXUnitWrapper;
    using Fact = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
#else
    using UnitDriven;
    using Assert = Ninject.SilverlightTests.AssertXUnitWrapper;
    using Fact = UnitDriven.TestMethodAttribute;
#endif
#else
    using Ninject.Tests.MSTestAttributes;
    using Xunit;
#endif

#if !SILVERLIGHT
    [TestClass]
    public class GenericBindingGeneratorTests
    {
        [Fact]
        public void OpenGenericsAreFound()
        {
            IKernel kernel = new StandardKernel();
            kernel.Scan(
                x =>
                {
                    x.FromCallingAssembly();
                    x.BindWith(new GenericBindingGenerator(typeof(IGenericView<>)));
                });
            object target1 = kernel.Get<IGenericView<IDefaultConvention>>();
            object target2 = kernel.Get<IGenericView<string>>();

            Assert.IsAssignableFrom<DefaultConventionView>(target1);
            Assert.IsAssignableFrom<StringView>(target2);
        }
    }
#endif
}