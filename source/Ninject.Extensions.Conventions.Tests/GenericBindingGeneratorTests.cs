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
                    x.CallingAssembly();
                    x.Using( new GenericBindingGenerator( typeof (IGenericView<>) ) );
                } );
            object target = kernel.Get<IGenericView<IDefaultConvention>>();
            Assert.IsAssignableFrom<DefaultConventionView>( target );
            target = kernel.Get<IGenericView<string>>();
            Assert.IsAssignableFrom<StringView>( target );
        }
    }
}