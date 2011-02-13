namespace Ninject.Extensions.Conventions
{
#if SILVERLIGHT
#else
    using System.Reflection;
    using System.Linq;
    using Fakes;
    using Tests.MSTestAttributes;
    using Xunit;
    using Xunit.Should;

    [TestClass]
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
                kernel.GetBindings(typeof(IDefaultView)).Count().ShouldBe(1);
                kernel.Get<IDefaultView>().ShouldBeInstanceOf<DefaultView>();
                scanner.BindingGenerators.Count.ShouldBe(1);
                scanner.BindingGenerators[0].ShouldBeInstanceOf<DefaultBindingGenerator>();
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
                kernel.GetBindings(typeof(IDefaultView)).Count().ShouldBe(1);
                kernel.Get<IDefaultView>().ShouldBeInstanceOf<DefaultView>();
                scanner.BindingGenerators.Count.ShouldBe(1);
                scanner.BindingGenerators[0].ShouldBeInstanceOf<RegexBindingGenerator>();
            }
        }
    }
#endif
}