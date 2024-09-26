using System;
using System.IO;
using System.Threading.Tasks;

namespace Codeworx.Rest.UnitTests.Api.Contract
{
    [RestRoute("api/mimetypes")]
    public interface IMimeTypeController
    {
        [RestGet("pdf")]
        [ResponseType(200, typeof(Stream), "application/pdf")]
        [ResponseType(400, typeof(string), "text/plain")]
        Task<Stream> GetPdf();

        [RestGet("octet")]
        [ResponseType(200, typeof(Stream), "application/octet-stream")]
        Task<Stream> GetOctet();


        [RestGet("plain-stream")]
        [ResponseType(200, typeof(Stream), "text/plain")]
        Task<Stream> GetPlainStream();

        [RestGet("plain")]
        [ResponseType(200, typeof(string), "text/plain")]
        Task<string> GetPlainString();


        [RestPut()]
        [ResponseType(200, typeof(string), "text/plain")]
        Task<string> PutText([BodyMember("text/plain")] string body);

        [RestPut("uri")]
        [ResponseType(200, typeof(string), "text/plain")]
        Task<string> PutUri([BodyMember("application/json", "text/plain")] Uri body);
    }
}