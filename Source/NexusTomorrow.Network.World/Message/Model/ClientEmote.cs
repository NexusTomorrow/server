using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientEmote, Direction.ClientToServer)]
public sealed partial class ClientEmote
{
    [UInt(14)] public ushort EmoteId { get; private set; }
    [UInt(32)] public uint Seed { get; set; }
    [UInt(32)] public uint TargetUnitId { get; set; }
    [UInt(1)] public bool Targeted { get; set; }
    [UInt(1)] public bool Silent { get; set; }
}
