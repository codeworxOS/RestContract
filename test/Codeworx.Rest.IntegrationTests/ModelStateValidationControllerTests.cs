using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.TestServerUtilities;
using Xunit;

namespace Codeworx.Rest.UnitTests
{
    public class ModelStateValidationControllerTests : TestServerTestsBase<StartupWithModelValidation>
    {
        ////[RestRoute("api/model-state-validation")]
        ////public interface IModelStateValidationController
        ////{
        ////    [RestPost("guid-query")]
        ////    Task GuidQueryParameterAsync([QueryMember] Guid id);

        ////    [RestPost("none-null-body")]
        ////    Task ItemBodyNotNullAsync([BodyMember] Item item);

        ////    [RestPost("nullable-body")]
        ////    Task ItemBodyNullableAsync([BodyMember] Item? item);
        ////}

        [Fact]
        public async Task CheckWrongQueryParameterData_Expects400()
        {
            var response = await HttpClient.PostAsync("https://localhost/api/model-state-validation/guid-query?id=abcdefg", new StringContent(""));
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CheckMissingQueryParameter_Expects400()
        {
            var response = await HttpClient.PostAsync("https://localhost/api/model-state-validation/guid-query", new StringContent(""));
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CheckMissingNullableQueryParameter_Expects200()
        {
            var response = await HttpClient.PostAsync("https://localhost/api/model-state-validation/nullable-guid-query", new StringContent(""));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CheckEmptyNoneNullBodyMember_Expects400()
        {
            var body = new StringContent("");
            body.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await HttpClient.PostAsync("https://localhost/api/model-state-validation/none-null-body", body);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

#if NET8_0_OR_GREATER
        [Fact]
        public async Task CheckNullableBodyMember_Expects200()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost/api/model-state-validation/nullable-body");

            var body = new ByteArrayContent(new byte[0]);
            body.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            request.Content = body;

            var response = await HttpClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
#endif

        [Fact]
        public async Task CheckNullableBodyMemberWithAttribute_Expects200()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost/api/model-state-validation/nullable-body-by-attribute");

            var body = new ByteArrayContent(new byte[0]);
            body.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            request.Content = body;

            var response = await HttpClient.SendAsync(request);

            var responseBody = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}