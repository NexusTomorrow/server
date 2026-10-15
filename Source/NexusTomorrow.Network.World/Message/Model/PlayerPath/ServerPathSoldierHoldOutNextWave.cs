using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathSoldierHoldOutNextWave)]
[WireMessage(GameMessageOpcode.ServerPathSoldierHoldOutNextWave, Direction.ServerToClient)]
public sealed partial class ServerPathSoldierHoldOutNextWave : IWritable
{
    [UInt(14)] public ushort PathSoldierEventId { get; set; }
    [UInt(32)] public uint WaveIndex { get; set; }
    [UInt(1)] public bool IsBoss { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(PathSoldierEventId, 14);
        writer.Write(WaveIndex);
        writer.Write(IsBoss);
        ;
    }
}
