using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.Client0151, Direction.ClientToServer)]
public sealed partial class Client0151
{
    [UInt(3)] public byte Unknown0 { get; private set; }
    [UInt(3)] public byte Unknown1 { get; private set; }
    [UInt(32)] public uint Unknown2 { get; private set; }
}
