using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathSoliderWhackAMoleBurrows)]
[WireMessage(GameMessageOpcode.ServerPathSoliderWhackAMoleBurrows, Direction.ServerToClient)]
public sealed partial class ServerPathSoldierWhackAMoleBurrows : IWritable
{
    [UInt(32)] public uint DelayUntilPop { get; set; }
    [Array(CountBits = 32), UInt(32)] public uint[] UnitIds { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(DelayUntilPop);
        writer.Write(UnitIds.Length);
        foreach (uint unitId in UnitIds)
        {
            writer.Write(unitId);
        }
    }
}
