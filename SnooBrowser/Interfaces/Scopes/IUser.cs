using System.Threading.Tasks;
using SnooBrowser.Structures.User;

namespace SnooBrowser.Interfaces.Scopes
{
    public interface IUser
    {
        public Task<GetAboutUserResponse> GetAboutUser(string username);
    }
}
