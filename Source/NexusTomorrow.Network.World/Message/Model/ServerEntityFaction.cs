using NexusTomorrow.Game.Static.Reputation;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerEntityFaction)]
[WireMessage(GameMessageOpcode.ServerEntityFaction, Direction.ServerToClient)]
public sealed partial class ServerEntityFaction : IWritable
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(14)] public Faction Faction { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(Faction, 14u);
    }
}
