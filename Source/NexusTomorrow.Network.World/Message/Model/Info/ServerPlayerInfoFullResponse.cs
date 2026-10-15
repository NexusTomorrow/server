using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using Path = NexusTomorrow.Game.Static.PlayerPath.Path;

namespace NexusTomorrow.Network.World.Message.Model.Info;

[Message(GameMessageOpcode.ServerPlayerInfoFullResponse)]
[WireMessage(GameMessageOpcode.ServerPlayerInfoFullResponse, Direction.ServerToClient)]
public sealed partial class ServerPlayerInfoFullResponse : IWritable
{
    public PlayerInfoBase BaseData { get; set; }
    [UInt(1)] public bool IsClassPathSet { get; set; } = true;
    [UInt(3)] public Path Path { get; set; }
    [UInt(8)] public Class Class { get; set; }
    [Pad(6)] public Pad ClassHigh { get; }
    [UInt(32)] public uint Level { get; set; }
    [UInt(1)] public bool IsLastLoggedOnInDaysSet { get; set; }
    [Float32] public float LastLoggedInDays { get; set; }

    public void Write(GamePacketWriter writer)
    {
        BaseData.Write(writer);
        writer.Write(IsClassPathSet);
        writer.Write(Path, 3u);
        writer.Write(Class, 14u);
        writer.Write(Level);
        writer.Write(IsLastLoggedOnInDaysSet);
        writer.Write(LastLoggedInDays);
    }
}
