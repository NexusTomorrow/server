using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.Client0850, Direction.ClientToServer)]
public sealed partial class Client0850
{
    [UInt(32)] public uint Unknown0 { get; private set; }
    [UInt(32)] public uint Unknown1 { get; private set; }
    [UInt(32)] public uint Unknown2 { get; private set; }
}
