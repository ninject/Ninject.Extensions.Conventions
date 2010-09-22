namespace Ninject.Extensions.Conventions.Tests.Fakes
{
    internal interface IGenericView
    {
    }

    internal interface IGenericView<T> : IGenericView
    {
    }
}