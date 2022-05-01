using FruityFoundation.Base.Extensions;
using SnooBrowser.Things;

namespace SnooBrowser.Util
{
    public static class ThingsExtensions
    {
        public static bool IsAutoMod(this Author author) =>
            author.Name.EqualsIgnoreCase("AutoModerator");
    }
}
