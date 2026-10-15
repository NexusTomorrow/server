using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerGroupUpdatePlayerRealm)]
[WireMessage(GameMessageOpcode.ServerGroupUpdatePlayerRealm, Direction.ServerToClient)]
public sealed partial class ServerGroupUpdatePlayerRealm : IWritable
{
    [UInt(64)] public ulong GroupId { get; set; }
    public Identity TargetPlayerIdentity { get; set; }
    [UInt(14)] public uint RealmId { get; set; }
    [UInt(15)] public uint ZoneId { get; set; }
    [UInt(32)] public uint MapId { get; set; }
    [UInt(32)] public uint PhaseId { get; set; }
    [UInt(1)] public bool IsSyncdToGroup { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(GroupId);
        TargetPlayerIdentity.Write(writer);
        writer.Write(RealmId, 14);
        writer.Write(ZoneId, 15);
        writer.Write(MapId);
        writer.Write(PhaseId);
        writer.Write(IsSyncdToGroup);
    }
}
