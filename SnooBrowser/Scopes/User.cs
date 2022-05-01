using System.Threading.Tasks;
using Flurl.Http;
using SnooBrowser.Interfaces.Scopes;
using SnooBrowser.Structures.User;
using SnooBrowser.Util;

namespace SnooBrowser.Scopes
{
    public class User : BaseBrowserConsumer, IUser
    {
        public User(ISnooBrowser browser)
            : base(browser)
        { }

        public Task<GetAboutUserResponse> GetAboutUser(string username) =>
            _browser
                .CreateRequest("user")
                .AppendPathSegment(username)
                .AppendPathSegment("about")
                .GetJsonAsync<GetAboutUserResponse>();
    }
}
