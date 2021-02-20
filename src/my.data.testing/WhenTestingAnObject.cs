using Xunit.Abstractions;

namespace my.data.testing
{

    public class WhenTestingAnObject: WhenTestingAUnit<object>
    {
        public WhenTestingAnObject(ITestOutputHelper output): base(output)
        {

        }
    }
}
