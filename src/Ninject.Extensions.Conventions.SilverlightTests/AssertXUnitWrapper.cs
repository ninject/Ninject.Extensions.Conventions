namespace Ninject.SilverlightTests
{
    using System;
    using System.Globalization;
#if SILVERLIGHT_MSTEST
    using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
#else
    using Assert = UnitDriven.Assert;
#endif

    public class AssertXUnitWrapper
    {
        public static void DoesNotThrow(Action action)
        {
            try
            {
                action();
            }
            catch (Exception)
            {
                Assert.Fail("Expected no exception");
            }
        }

        public static void Throws<T>(Action action)
            where T : Exception
        {
            try
            {
                action();
                Assert.Fail(string.Format(CultureInfo.InvariantCulture, "Expected excpetion {0} did not occur!", typeof(T).Name));
            }
            catch (T)
            {
            }
        }

        public static void IsAssignableFrom<T>(object instance)
        {
            Assert.IsTrue(instance.GetType().IsAssignableFrom(typeof(T)));
        }
    }
}
