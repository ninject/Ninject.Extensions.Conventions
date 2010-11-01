namespace Ninject.Extensions.Conventions.Fakes
{
    public class DefaultConvention : IDefaultConvention
    {
        public string Name
        {
            get { return GetType().Name; }
        }
    }
}