using System.Collections.Generic;

namespace Codeworx.Rest
{
    public interface IAdditionalDataProvider
    {
        IDictionary<string, object> GetValues();
    }
}
