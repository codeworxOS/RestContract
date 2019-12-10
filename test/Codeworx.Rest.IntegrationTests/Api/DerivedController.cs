using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Codeworx.Rest.UnitTests.Api
{
    public class DerivedController : BaseController, IDerivedController
    {
        public Task<string> Derived()
        {
            return Task.FromResult(nameof(IDerivedController.Derived));
        }
    }
}