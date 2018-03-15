using System.Threading.Tasks;
using TestManager.Lib.ViewModels;

namespace TestManager.Lib.Services
{
    interface IDataRetriever
    {
        Task<PostmanCollection[]> GetCollections(string path);
    }
}
