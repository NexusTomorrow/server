using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Support;

/// <summary>
/// Sends the player's suggestion, the request one of the client's chat commands makes.
/// </summary>
[WireMessage(GameMessageOpcode.Client0833, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class SuggestionSubmit
{
    [WideString] public string Unknown0 { get; private set; } = "";
}
