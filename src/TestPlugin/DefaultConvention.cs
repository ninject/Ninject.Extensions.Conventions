namespace TestPlugin
{
    using Ninject.Extensions.Conventions.Fakes;

    /// <summary>
    /// The plugins implementation of DefaultConvention
    /// </summary>
    public class DefaultConvention : IDefaultConvention
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name of the convention.</value>
        public string Name
        {
            get
            {
                return "DefaultConventionFromPlugin";
            }
        }
    }
}