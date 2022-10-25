using System;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.UnitTests.Api.Contract.Model;
using Codeworx.Rest.UnitTests.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Net.Http.Headers;

namespace Codeworx.Rest.UnitTests.Api
{
    public class FileStreamController : IFileStreamController
    {
        public FileStreamController()
        {
        }

        public async Task<Stream> GetFileStream()
        {
            var testFilePath = ItemsGenerator.CreateTestFilename();
            await ItemsGenerator.CreateTestFile(testFilePath);
            var fileStream = new FileStream(testFilePath, FileMode.Open, FileAccess.Read, FileShare.None, 4096, FileOptions.DeleteOnClose);
            return fileStream;
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

        public async Task<string> UpdateFile(Stream stream)
        {
            var testFilePath = ItemsGenerator.CreateTestFilename();
            try
            {
                using (var fileStream = File.OpenWrite(testFilePath))
                {
                    await stream.CopyToAsync(fileStream);
                }

                var fileContent = File.ReadAllText(testFilePath);
                return fileContent;
            }
            finally
            {
                ItemsGenerator.DeleteFile(testFilePath);
            }
        }

        public async Task<string> UpdateFileById(Stream stream, Guid id)
        {
            var testFilePath = ItemsGenerator.CreateTestFilename();
            try
            {
                using (var fileStream = File.OpenWrite(testFilePath))
                {
                    await stream.CopyToAsync(fileStream);
                }

                var fileContent = File.ReadAllText(testFilePath);
                return fileContent;
            }
            finally
            {
                ItemsGenerator.DeleteFile(testFilePath);
            }
        }

        public async Task<string> UpdateStreamItem(StreamItem streamItem)
        {
            var testFilePath = ItemsGenerator.CreateTestFilename();
            try
            {
                var stream = streamItem.Stream;
                using (var fileStream = File.OpenWrite(testFilePath))
                {
                    await stream.CopyToAsync(fileStream);
                }

                var fileContent = File.ReadAllText(testFilePath);
                return fileContent;
            }
            finally
            {
                ItemsGenerator.DeleteFile(testFilePath);
            }
        }

        [HttpPost("uploadbla")]
        [RequestFormat(MediaTypeNames.Application.Octet)]
        public Task Whatever([FromBody] Stream file)
        {
            return Task.CompletedTask;
        }
    }
}