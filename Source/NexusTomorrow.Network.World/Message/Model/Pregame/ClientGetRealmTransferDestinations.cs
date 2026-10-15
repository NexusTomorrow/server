using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

// Sent when the lua function CharacterScreenLib::GetRealmTransferDestinations is called
// Response from ServerTransferDestinationRealmList
[WireMessage(GameMessageOpcode.ClientGetRealmTransferDestinations, Direction.ClientToServer)]
public sealed partial class ClientGetRealmTransferDestinations
{
    [UInt(1)] public bool Unknown0 { get; private set; }
}
