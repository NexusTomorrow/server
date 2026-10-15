using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerEntitlement)]
[WireMessage(GameMessageOpcode.ServerEntitlement, Direction.ServerToClient)]
public sealed partial class ServerEntitlement : IWritable
{
    [UInt(14)] public EntitlementType Entitlement { get; set; }
    [UInt(32)] public uint Count { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Entitlement, 14u);
        writer.Write(Count);
    }
}
