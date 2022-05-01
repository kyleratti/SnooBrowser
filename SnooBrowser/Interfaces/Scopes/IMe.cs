using System.Threading.Tasks;
using SnooBrowser.Structures.Me;

namespace SnooBrowser.Interfaces.Scopes
{
    public interface IMe
    {
        public Task<GetMeResponse> GetMe();
    }
}
