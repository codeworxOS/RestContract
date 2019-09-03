using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.UnitTests.Dao;
using Xunit;

namespace Codeworx.Rest.UnitTests
{
    public class StatusCodeTests : TestServerTestsBase
    {
        private readonly IStatusCodeController _controller;

        public StatusCodeTests()
        {
            _controller = new StatusCodeDao(RestOptions);
        }

        [Fact]
        public async Task TestSuccess()
        {
            var result = await _controller.GetValueSuccess();
            Assert.True(result);
        }

        [Fact]
        public async Task TestException()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _controller.GetValueException());
        }

    }
}
