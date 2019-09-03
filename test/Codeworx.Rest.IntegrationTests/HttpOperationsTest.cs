using System;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.UnitTests.Dao;
using Xunit;

namespace Codeworx.Rest.UnitTests
{
    public class HttpOperationsTest : TestServerTestsBase
    {
        private readonly IHttpOperationsController _controller;
        private readonly IPublicMethodsWithNoInterfaceController _publicMethodsWithNoInterfaceController;
        public HttpOperationsTest()
        {
            _controller = new HttpOperationsDao(RestOptions);
            _publicMethodsWithNoInterfaceController = new PublicMethodsWithNoInterfaceDao(RestOptions);
        }

        [Fact]
        public async Task TestHttpGet()
        {
            var result = await _controller.Get();

            var methodName = nameof(IHttpOperationsController.Get);
            Assert.Equal(methodName, result);
        }

        [Fact]
        public async Task TestHttpPut()
        {
            var result = await _controller.Put();

            var methodName = nameof(IHttpOperationsController.Put);
            Assert.Equal(methodName, result);
        }

        [Fact]
        public async Task TestHttpPost()
        {
            var result = await _controller.Post();

            var methodName = nameof(IHttpOperationsController.Post);
            Assert.Equal(methodName, result);
        }

        [Fact]
        public async Task TestHttpDelete()
        {
            var result = await _controller.Delete();

            var methodName = nameof(IHttpOperationsController.Delete);
            Assert.Equal(methodName, result);
        }

        [Fact]
        public async Task TestMissingHttpAttribute()
        {
            await Assert.ThrowsAsync<NotSupportedException>(async () =>  await _controller.MissingOperation());
        }

        [Fact]
        public async Task TestMissingInterface()
        {
            var result = await _publicMethodsWithNoInterfaceController.MethodWithInterface();
            Assert.True(result);
        }
    }
}
