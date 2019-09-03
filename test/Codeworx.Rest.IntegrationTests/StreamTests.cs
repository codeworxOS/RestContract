using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.UnitTests.Dao;

namespace Codeworx.Rest.UnitTests
{
    public class StreamTests: TestServerTestsBase
    {
        private readonly IFileStreamController _controller;

        public StreamTests()
        {
            _controller = new FileStreamDao(RestOptions);
        }
    }
}
