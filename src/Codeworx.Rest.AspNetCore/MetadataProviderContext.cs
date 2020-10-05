using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Codeworx.Rest.AspNetCore
{
    public class MetadataProviderContext
    {
        public MetadataProviderContext()
        {
            this.ActionsToRemove = new HashSet<ActionModel>();
        }

        public HashSet<ActionModel> ActionsToRemove { get; }
    }
}
