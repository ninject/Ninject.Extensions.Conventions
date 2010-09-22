namespace Ninject.Extensions.Conventions.Tests.Fakes
{
    public class DefaultConvention : IDefaultConvention
    {
        #region Implementation of IDefaultConvention

        public string Name
        {
            get { return GetType().Name; }
        }

        #endregion
    }
}