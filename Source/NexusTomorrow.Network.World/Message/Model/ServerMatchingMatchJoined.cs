using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerMatchingMatchJoined, Direction.ServerToClient)]
public sealed partial class ServerMatchingMatchJoined
{
    [UInt(14)] public uint MatchingGameMapId { get; set; } // Id of map the player has just joined
}
