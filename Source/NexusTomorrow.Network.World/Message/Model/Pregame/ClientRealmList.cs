using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

// Sent when the lua function CharacterScreenLib::ExitToRealmSelect is called
[WireMessage(GameMessageOpcode.ClientRealmList, Direction.ClientToServer)]
public sealed partial class ClientRealmList
{
}
