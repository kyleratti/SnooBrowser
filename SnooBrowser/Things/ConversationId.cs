namespace SnooBrowser.Things
{
    public class ConversationId
    {
        private ConversationId(string id)
        {
            Id = id;
        }

        public string Id { get; }

        public static ConversationId Create(string id) =>
            new(id);
    }
}
