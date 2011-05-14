namespace Ninject.Extensions.Conventions
{
    using System.Linq;
    using Ninject.Extensions.Conventions.Fakes;
    using Ninject.Infrastructure;
    using Xunit;
    using Xunit.Should;

    public class RegexBindingGeneratorTests
    {
        [Fact]
        public void CanResolveMatchInMiddleOfInterface()
        {
            var regexBindingGenerator = new RegexBindingGenerator("(I)(?<name>.+)(View)");
            using (IKernel kernel = new StandardKernel())
            {
                regexBindingGenerator.Process(typeof(DefaultView), StandardScopeCallbacks.Transient, kernel);
                kernel.GetBindings(typeof(IDefaultView)).Count().ShouldBe(1);
                kernel.Get<IDefaultView>().ShouldBeInstanceOf<DefaultView>();
            }
        }

        [Fact]
        public void CanResolveMatchAtEndOfInterface()
        {
            var regexBindingGenerator = new RegexBindingGenerator("(I)(?<name>.+)");
            using (IKernel kernel = new StandardKernel())
            {
                regexBindingGenerator.Process(typeof(DefaultView), StandardScopeCallbacks.Transient, kernel);
                kernel.GetBindings(typeof(IDefaultView)).Count().ShouldBe(1);
                kernel.Get<IDefaultView>().ShouldBeInstanceOf<DefaultView>();
                kernel.GetBindings(typeof(IDefaultConvention)).Count().ShouldBe(1);
                kernel.Get<IDefaultConvention>().ShouldBeInstanceOf<DefaultView>();
            }
        }

        [Fact]
        public void CanResolveDetailedMatchAtEndOfInterface()
        {
            var regexBindingGenerator = new RegexBindingGenerator("(IDefault)(?<name>.+)");
            using (IKernel kernel = new StandardKernel())
            {
                regexBindingGenerator.Process(typeof(DefaultView), StandardScopeCallbacks.Transient, kernel);
                kernel.GetBindings(typeof(IDefaultView)).Count().ShouldBe(1);
                kernel.Get<IDefaultView>().ShouldBeInstanceOf<DefaultView>();
                kernel.GetBindings(typeof(IDefaultConvention)).Count().ShouldBe(1);
                kernel.Get<IDefaultConvention>().ShouldBeInstanceOf<DefaultView>();
            }
        }
    }
}