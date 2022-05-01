using System;
using System.IO;

namespace SnooBrowser.Things
{
	/// <summary>
	/// The Fullname Type
	/// </summary>
    public enum FullnameType
    {
        /// <summary>
        /// Comment
        /// </summary>
        Comment = 1,
        /// <summary>
        /// Account
        /// </summary>
        Account = 2,
        /// <summary>
        /// Link
        /// </summary>
        Link = 3,
        /// <summary>
        /// Message
        /// </summary>
        Message = 4,
        /// <summary>
        /// Subreddit
        /// </summary>
        Subreddit = 5,
        /// <summary>
        /// Award
        /// </summary>
        Award = 6
    }

    /// <summary>
    /// Fullname
    /// </summary>
    public class Fullname
    {
        private Fullname(FullnameType fnType, string id)
        {
            Type = fnType;
            ShortId = id;
        }

        /// <summary>
        /// The Type of Fullname this is.
        /// </summary>
        public FullnameType Type { get; }
        /// <summary>
        /// The Short ID of this fullname. 
        /// </summary>
        /// <example>"12jir3"</example>
        public string ShortId { get; }

        /// <summary>
        /// The prefix of this Thing, excluding the trailing underscore (<c>_</c>)
        /// </summary>
        /// <example>t4</example>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string Prefix
        {
            get
            {
                return Type switch
                {
                    FullnameType.Comment => "t1",
                    FullnameType.Account => "t2",
                    FullnameType.Link => "t3",
                    FullnameType.Message => "t4",
                    FullnameType.Subreddit => "t5",
                    FullnameType.Award => "t6",
                    _ => throw new ArgumentOutOfRangeException(nameof(Type))
                };
            }
        }

        /// <summary>
        /// The full ID of this Thing.
        /// </summary>
        /// <example>t4_12jd8</example>
        public string FullId => $"{Prefix}_{ShortId}";

        /// <summary>
        /// Whether this Fullname is a Comment.
        /// </summary>
        public bool IsComment => Type is FullnameType.Comment;
        /// <summary>
        /// Whether this Fullname is an Account.
        /// </summary>
        public bool IsAccount => Type is FullnameType.Account;
        /// <summary>
        /// Whether this Fullname is a Link.
        /// </summary>
        public bool IsLink => Type is FullnameType.Link;
        /// <summary>
        /// Whether this Fullname is a Message.
        /// </summary>
        public bool IsMessage => Type is FullnameType.Message;
        /// <summary>
        /// Whether this Fullname is a Subreddit.
        /// </summary>
        public bool IsSubreddit => Type is FullnameType.Subreddit;
        /// <summary>
        /// Whether this Fullname is an Award.
        /// </summary>
        public bool IsAward => Type is FullnameType.Award;

        /// <summary>
        /// Create a Fullname from a string.
        /// </summary>
        public static Fullname FromString(string fullname)
        {
            if (fullname.Length < 4)
                throw new InvalidDataException($"Provided string must be at least 4 characters in order to be a Fullname: {fullname}");

            var id = fullname[3..];
            return fullname[..3] switch
            {
                "t1_" => NewComment(id),
                "t2_" => NewAccount(id),
                "t3_" => NewLink(id),
                "t4_" => NewMessage(id),
                "t5_" => NewSubreddit(id),
                "t6_" => NewAward(id),
                _ => throw new ArgumentOutOfRangeException(nameof(fullname), fullname)
            };
        }

        /// <summary>
        /// Create a Fullname of the specified type using the provided short ID.
        /// </summary>
        /// <param name="fnType">The Fullname Type</param>
        /// <param name="shortId">The short ID (e.g., if the Fullname is t1_12ejk, the short ID is "12ejk").</param>
        public static Fullname FromType(FullnameType fnType, string shortId) =>
            new(fnType, shortId);

        /// <summary>
        /// Create a new Comment from the specified Short ID.
        /// </summary>
        public static Fullname NewComment(string id) =>
            new(FullnameType.Comment, id);

        /// <summary>
        /// Create a new Account from the specified Short ID.
        /// </summary>
        public static Fullname NewAccount(string id) =>
            new(FullnameType.Account, id);

        /// <summary>
        /// Create a new Link from the specified Short ID.
        /// </summary>
        public static Fullname NewLink(string id) =>
            new(FullnameType.Link, id);

        /// <summary>
        /// Create a new Message from the specified Short ID.
        /// </summary>
        public static Fullname NewMessage(string id) =>
            new(FullnameType.Message, id);

        /// <summary>
        /// Create a new Subreddit from the specified Short ID.
        /// </summary>
        public static Fullname NewSubreddit(string id) =>
            new(FullnameType.Subreddit, id);

        /// <summary>
        /// Create a new Award from the specified Short ID.
        /// </summary>
        public static Fullname NewAward(string id) =>
            new(FullnameType.Award, id);
    }
}
