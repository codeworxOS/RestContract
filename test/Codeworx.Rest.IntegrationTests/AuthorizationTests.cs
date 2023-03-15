using System.Net;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Xunit;

namespace Codeworx.Rest.UnitTests
{
    public class AuthorizationTests : TestServerTestsBase
    {
        [Fact]
        public async Task TestAnonymousController()
        {
            await Client<IAuthorizedController>(FormatterSelection.Json).AnonymousAsync();
            Assert.True(true);
        }

        [Fact]
        public async Task TestAuthorizedController()
        {
            var ex = await Assert.ThrowsAsync<UnexpectedHttpStatusCodeException>(async () =>
            {
                await Client<IAuthorizedController>(FormatterSelection.Json).DenyAnonymousAsync();
            });

            Assert.Equal(HttpStatusCode.Unauthorized, ex.StatusCode);
        }

        [Fact]
        public async Task TestAnonymousAction()
        {
            await Client<IAuthorizedAction>(FormatterSelection.Json).AnonymousAsync();
            Assert.True(true);
        }

        [Fact]
        public async Task TestAuthorizedAction()
        {
            var ex = await Assert.ThrowsAsync<UnexpectedHttpStatusCodeException>(async () =>
            {
                await Client<IAuthorizedAction>(FormatterSelection.Json).DenyAnonymousAsync();
            });

            Assert.Equal(HttpStatusCode.Unauthorized, ex.StatusCode);
        }

    }
}