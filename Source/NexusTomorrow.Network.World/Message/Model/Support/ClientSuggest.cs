using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Support;

// From the /suggest command
[WireMessage(GameMessageOpcode.ClientSuggest, Direction.ClientToServer)]
public sealed partial class ClientSuggest
{
    [WideString] public string SuggestionText { get; private set; }
}
