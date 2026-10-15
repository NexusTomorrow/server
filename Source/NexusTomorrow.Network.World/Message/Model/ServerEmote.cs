using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerEmote, Direction.ServerToClient)]
public sealed partial class ServerEmote
{
    [UInt(14)] public ushort EmotesId { get; set; }
    [UInt(32)] public uint Seed { get; set; }
    [UInt(32)] public uint SourceUnitId { get; set; }
    [UInt(32)] public uint TargetUnitId { get; set; }
    [UInt(1)] public bool Targeted { get; set; }
    [UInt(1)] public bool Silent { get; set; }
}
