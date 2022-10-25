﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract.Model;

namespace Codeworx.Rest.UnitTests.Api.Contract
{
    [RestRoute("api/FileStream")]
    [RequestFormat(MediaTypeNames.Application.Octet)]
    public interface IFileStreamController
    {
        [RestGet("Stream")]
        Task<Stream> GetFileStream();

        [RestGet("StreamItem")]
        Task<StreamItem> GetStreamItem();

        [RestPut("Stream")]
        [RequestFormat(MediaTypeNames.Application.Octet)]
        Task<string> UpdateFile([BodyMember] Stream fileStream);

        [RestPut("Stream/{id}")]
        Task<string> UpdateFileById([BodyMember] Stream stream, Guid id);

        [RestPut("StreamItem")]
        Task<string> UpdateStreamItem(StreamItem streamItem);
    }
}