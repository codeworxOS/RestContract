using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract.Model;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.Client;

namespace Codeworx.Rest.UnitTests.Generated
{
    public class FileStreamControllerClient : RestClient<IFileStreamController>, IFileStreamController
    {
        public FileStreamControllerClient(RestOptions<IFileStreamController> options): base(options)
        {
        }

        public FileStreamControllerClient(RestOptions options): base(options)
        {
        }

        public Task<Stream> GetFileStream()
        {
            return CallAsync(c => c.GetFileStream());
        }

        public Task<string> UpdateFile(Stream fileStream)
        {
            return CallAsync(c => c.UpdateFile(fileStream));
        }

        public Task<string> UpdateFileById(Stream stream, Guid id)
        {
            return CallAsync(c => c.UpdateFileById(stream, id));
        }

        public Task<StreamItem> GetStreamItem()
        {
            return CallAsync(c => c.GetStreamItem());
        }

        public Task<string> UpdateStreamItem(StreamItem streamItem)
        {
            return CallAsync(c => c.UpdateStreamItem(streamItem));
        }
    }
}