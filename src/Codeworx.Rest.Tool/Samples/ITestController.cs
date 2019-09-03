using System.Threading.Tasks;

namespace Codeworx.Rest.Tool.Samples
{
    public interface ITestController
    {
        Task<SampleType> GetDataAsync(int id);

        Task SaveData(SampleType test);
    }
}