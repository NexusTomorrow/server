using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerMatchingMatchEntered, Direction.ServerToClient)]
public sealed partial class ServerMatchingMatchEntered
{
    [UInt(32)] public uint MatchingGameMapId { get; set; }
}
