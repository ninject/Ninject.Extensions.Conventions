namespace Ninject.Extensions.Conventions
{
#if !SILVERLIGHT
    using System.Reflection;
    using System.Linq;
    using Fakes;

    using FluentAssertions;

    using Xunit;

    public class AssemblyScannerTestsWhereTypeInheritsFrom
    {
        private static readonly MethodInfo ProcessMethod =
            typeof (AssemblyScanner).GetMethod("Process",
                                               BindingFlags.Instance | BindingFlags.NonPublic |
                                               BindingFlags.DeclaredOnly,
                                               null,
                                               new [] {typeof (IKernel)}, null);

        [Fact]
        public void DefaultConventionsApply()
        {
            using (IKernel kernel = new StandardKernel())
            {
                var scanner = new AssemblyScanner();
                scanner.From(typeof(DefaultView).Assembly);
                scanner.WhereTypeInheritsFrom<IDefaultView>();
                scanner.BindWithDefaultConventions();
                ProcessMethod.Invoke(scanner, new[]{kernel});
                kernel.GetBindings(typeof(IDefaultView)).Count().Should().Be(1);
                kernel.Get<IDefaultView>().Should().BeOfType<DefaultView>();
                scanner.BindingGenerators.Count.Should().Be(1);
                scanner.BindingGenerators[0].Should().BeOfType<DefaultBindingGenerator>();
            }
        }

        [Fact]
        public void RegexBindingGeneratorCanBeUsed()
        {
            var regexBindingGenerator = new RegexBindingGenerator("(I)(?<name>.+)(View)");
            using (IKernel kernel = new StandardKernel())
            {
                var scanner = new AssemblyScanner();
                scanner.From(typeof(DefaultView).Assembly);
                scanner.WhereTypeInheritsFrom<IDefaultView>();
                scanner.BindWith(regexBindingGenerator);
                ProcessMethod.Invoke(scanner, new[]{kernel});
                kernel.GetBindings(typeof(IDefaultView)).Count().Should().Be(1);
                kernel.Get<IDefaultView>().Should().BeOfType<DefaultView>();
                scanner.BindingGenerators.Count.Should().Be(1);
                scanner.BindingGenerators[0].Should().BeOfType<RegexBindingGenerator>();
            }
        }
    }
#endif
}