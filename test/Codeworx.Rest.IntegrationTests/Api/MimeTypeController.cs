using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.UnitTests.Api.Contract.Model;
using Codeworx.Rest.UnitTests.Data;

namespace Codeworx.Rest.UnitTests.Api
{
    public class MimeTypeController : IMimeTypeController
    {
        public async Task<Stream> GetOctet()
        {
            var testFilePath = ItemsGenerator.CreateTestFilename();
            await ItemsGenerator.CreateTestFile(testFilePath);
            var fileStream = new FileStream(testFilePath, FileMode.Open, FileAccess.Read, FileShare.None, 4096, FileOptions.DeleteOnClose);
            return fileStream;
        }

        public async Task<Stream> GetPdf()
        {
            var testFilePath = ItemsGenerator.CreateTestFilename();
            await ItemsGenerator.CreateTestFile(testFilePath);
            var fileStream = new FileStream(testFilePath, FileMode.Open, FileAccess.Read, FileShare.None, 4096, FileOptions.DeleteOnClose);
            return fileStream;
        }

        public async Task<Stream> GetPlainStream()
        {
            await Task.Yield();
            return new MemoryStream(Encoding.UTF8.GetBytes("test"));
        }

        public Task<string> GetPlainString()
        {
            return Task.FromResult("test");
        }

        public async Task<StreamItem> GetStreamItem()
        {
            var testFilePath = ItemsGenerator.CreateTestFilename();
            await ItemsGenerator.CreateTestFile(testFilePath);
            var fileStream = new FileStream(testFilePath, FileMode.Open, FileAccess.Read, FileShare.None, 4096, FileOptions.DeleteOnClose);
            var streamItem = new StreamItem
            {
                Stream = fileStream,
                Id = ItemsGenerator.TestGuid
            };
            return streamItem;
        }

        public Task<string> PutText([BodyMember] string body)
        {
            return Task.FromResult(body);
        }

        public Task<string> PutUri([BodyMember] Uri body)
        {
            return Task.FromResult(body.ToString());
        }
    }
}