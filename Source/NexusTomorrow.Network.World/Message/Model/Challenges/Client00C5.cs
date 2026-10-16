using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Challenges;

[WireMessage(GameMessageOpcode.Client00C5, Direction.ClientToServer)]
public sealed partial class Client00C5
{
    [UInt(14)] public ushort Unknown0 { get; private set; }
    [UInt(5)] public byte Unknown1 { get; private set; }
    [UInt(32)] public uint Unknown2 { get; private set; }
}
