using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerUnitEnteredCombat)]
[WireMessage(GameMessageOpcode.ServerUnitEnteredCombat, Direction.ServerToClient)]
public sealed partial class ServerUnitEnteredCombat : IWritable
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(1)] public bool InCombat { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(InCombat);
    }
}
