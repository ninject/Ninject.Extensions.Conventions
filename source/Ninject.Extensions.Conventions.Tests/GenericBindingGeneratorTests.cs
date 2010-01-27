#region Using Directives

using Ninject.Extensions.Conventions.Tests.Fakes;
using Xunit;

#endregion

namespace Ninject.Extensions.Conventions.Tests
{
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
                    x.BindWith( new GenericBindingGenerator( typeof (IGenericView<>) ) );
                } );
            object target = kernel.Get<IGenericView<IDefaultConvention>>();
            Assert.IsAssignableFrom<DefaultConventionView>( target );
            target = kernel.Get<IGenericView<string>>();
            Assert.IsAssignableFrom<StringView>( target );
        }
    }
}