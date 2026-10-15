using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using Path = NexusTomorrow.Game.Static.PlayerPath.Path;

namespace NexusTomorrow.Network.World.Message.Model.Info;

[WireMessage(GameMessageOpcode.ServerPlayerInfoFullResponse, Direction.ServerToClient)]
public sealed partial class ServerPlayerInfoFullResponse
{
    public PlayerInfoBase BaseData { get; set; }
    [UInt(1)] public bool IsClassPathSet { get; set; } = true;
    [UInt(3)] public Path Path { get; set; }
    [UInt(8)] public Class Class { get; set; }
    [Pad(6)] public Pad ClassHigh { get; }
    [UInt(32)] public uint Level { get; set; }
    [UInt(1)] public bool IsLastLoggedOnInDaysSet { get; set; }
    [Float32] public float LastLoggedInDays { get; set; }
}
