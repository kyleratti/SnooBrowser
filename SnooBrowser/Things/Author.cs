using System;
using FruityFoundation.Base.Structures;
using Newtonsoft.Json;

namespace SnooBrowser.Things
{
    public record Author
    {
        /// <summary>
        /// The ID of the user.
        /// </summary>
        /// <remarks>If this <see cref="Author"/> has its <see cref="IsDeleted"/> as true, this will be an empty string.</remarks>
        [JsonProperty("id")] public string IdAsString = null!;
        public string Name = null!;
        public bool IsMod;
        public bool IsAdmin;
        public bool IsOp;
        public bool IsParticipant;
        public bool IsApproved;
        public bool IsHidden;
        public bool IsDeleted;

        public Maybe<long> UserId =>
            Maybe.Create(Convert.ToInt64(IdAsString), evalIsEmpty: () => IsDeleted);

        public static Author Create(long id, string name, bool isMod) =>
            new()
            {
                IdAsString = Convert.ToString(id),
                Name = name,
                IsMod = isMod
            };
    }
}
