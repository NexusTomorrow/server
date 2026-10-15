using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerEntityGroupAssociation, Direction.ServerToClient)]
public sealed partial class ServerEntityGroupAssociation
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(64)] public ulong GroupId { get; set; }
}
