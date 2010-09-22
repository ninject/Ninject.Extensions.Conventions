namespace Ninject.Extensions.Conventions.Tests.Fakes
{
    public class DefaultView : IDefaultView, IDefaultConvention
    {
        #region Implementation of IDefaultConvention

        public string Name
        {
            get { return GetType().Name; }
        }

        #endregion
    }
}