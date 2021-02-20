using System;
using Xunit;
using Xunit.Abstractions;

namespace my.data.testing
{
    /// <summary>
    /// Generioc testing base class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class WhenTestingAUnit<T> where T : class
    {
        /// <summary>
        /// the Unit under test
        /// </summary>
        protected T Unit;

        /// <summary>
        /// Provides console output options.
        /// </summary>
        protected readonly ITestOutputHelper Output;

        /// <summary>
        /// Constructor to provide test output helper. Creates the Unit as a default of its type. Override to add constructor options.
        /// </summary>
        /// <param name="output"></param>
        public WhenTestingAUnit(ITestOutputHelper output)
        {
            Output = output;
            Unit = Activator.CreateInstance<T>();
        }


        /// <summary>
        /// Overrideable basic test that the unit is available to test more specifically.
        /// </summary>
        [Fact]
        public virtual void ItShouldBeInstantiated()
        {
            Output.WriteLine($"Executing {nameof(ItShouldBeInstantiated)}");
            Assert.NotNull(Unit);
        }
    }
}
