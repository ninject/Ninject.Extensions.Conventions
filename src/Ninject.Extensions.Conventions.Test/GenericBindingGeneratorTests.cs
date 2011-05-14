namespace Ninject.Extensions.Conventions
{
    using Ninject.Extensions.Conventions.Fakes;
    using Xunit;

#if !SILVERLIGHT
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