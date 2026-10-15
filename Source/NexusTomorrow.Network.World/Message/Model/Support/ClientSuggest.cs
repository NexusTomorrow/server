using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Support;

// From the /suggest command
[Message(GameMessageOpcode.ClientSuggest)]
[WireMessage(GameMessageOpcode.ClientSuggest, Direction.ClientToServer)]
public sealed partial class ClientSuggest : IReadable
{
    [WideString] public string SuggestionText { get; private set; }

    public void Read(GamePacketReader reader)
    {
        SuggestionText = reader.ReadWideString();
    }
}
