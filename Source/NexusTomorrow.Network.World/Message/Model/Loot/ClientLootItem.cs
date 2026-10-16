using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Loot;

[WireMessage(GameMessageOpcode.Client014F, Direction.ClientToServer)]
public sealed partial class Client014F
{
    [UInt(32)] public uint Unknown0 { get; private set; }
    [UInt(32)] public uint Unknown1 { get; private set; }
    [UInt(1)] public bool Unknown2 { get; private set; }
}
