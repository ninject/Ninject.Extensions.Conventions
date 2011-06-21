namespace Ninject.Extensions.Conventions
{
    using System.Linq;

    using FluentAssertions;

    using Ninject.Extensions.Conventions.Fakes;
    using Ninject.Infrastructure;
    using Xunit;

    public class RegexBindingGeneratorTests
    {
        [Fact]
        public void CanResolveMatchInMiddleOfInterface()
        {
            var regexBindingGenerator = new RegexBindingGenerator("(I)(?<name>.+)(View)");
            using (IKernel kernel = new StandardKernel())
            {
                regexBindingGenerator.Process(typeof(DefaultView), StandardScopeCallbacks.Transient, kernel);
                kernel.GetBindings(typeof(IDefaultView)).Count().Should().Be(1);
                kernel.Get<IDefaultView>().Should().BeOfType<DefaultView>();
            }
        }

        [Fact]
        public void CanResolveMatchAtEndOfInterface()
        {
            var regexBindingGenerator = new RegexBindingGenerator("(I)(?<name>.+)");
            using (IKernel kernel = new StandardKernel())
            {
                regexBindingGenerator.Process(typeof(DefaultView), StandardScopeCallbacks.Transient, kernel);
                kernel.GetBindings(typeof(IDefaultView)).Count().Should().Be(1);
                kernel.Get<IDefaultView>().Should().BeOfType<DefaultView>();
                kernel.GetBindings(typeof(IDefaultConvention)).Count().Should().Be(1);
                kernel.Get<IDefaultConvention>().Should().BeOfType<DefaultView>();
            }
        }

        [Fact]
        public void CanResolveDetailedMatchAtEndOfInterface()
        {
            var regexBindingGenerator = new RegexBindingGenerator("(IDefault)(?<name>.+)");
            using (IKernel kernel = new StandardKernel())
            {
                regexBindingGenerator.Process(typeof(DefaultView), StandardScopeCallbacks.Transient, kernel);
                kernel.GetBindings(typeof(IDefaultView)).Count().Should().Be(1);
                kernel.Get<IDefaultView>().Should().BeOfType<DefaultView>();
                kernel.GetBindings(typeof(IDefaultConvention)).Count().Should().Be(1);
                kernel.Get<IDefaultConvention>().Should().BeOfType<DefaultView>();
            }
        }
    }
}