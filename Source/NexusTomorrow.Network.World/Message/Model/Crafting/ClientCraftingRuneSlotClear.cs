using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.Client085A, Direction.ClientToServer)]
public sealed partial class Client085A
{
    [UInt(64)] public ulong Unknown0 { get; private set; }
    [UInt(32)] public uint Unknown1 { get; private set; }
    [UInt(1)] public bool Unknown2 { get; private set; }
    [UInt(1)] public bool Unknown3 { get; private set; }
}
