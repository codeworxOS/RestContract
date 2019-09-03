using System;
using System.IO;

namespace Codeworx.Rest.UnitTests.Api.Contract.Model
{
    public class StreamItem : IDisposable
    {
        public Guid Id { get; set; }
        public Stream Stream { get; set; }

        public void Dispose()
        {
            Stream?.Dispose();
        }
    }
}
