using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Codeworx.Rest.Client;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.UnitTests.Model;

namespace Codeworx.Rest.UnitTests.Dao
{
    public class FileStreamDao : RestClient<IFileStreamController>, IFileStreamController
    {
        public FileStreamDao(RestOptions options)
            : base(options)
        {
        }

        
    }
}
