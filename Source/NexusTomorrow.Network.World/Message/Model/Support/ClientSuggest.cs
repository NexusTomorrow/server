using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Support
{
    // From the /suggest command
    [Message(GameMessageOpcode.ClientSuggest)]
    public class ClientSuggest : IReadable
    {
        public string SuggestionText { get; private set; }

        public void Read(GamePacketReader reader)
        {
            SuggestionText = reader.ReadWideString();
        }
    }
}
