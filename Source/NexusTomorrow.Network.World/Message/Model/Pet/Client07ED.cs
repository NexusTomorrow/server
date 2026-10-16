using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

[WireMessage(GameMessageOpcode.Client07ED, Direction.ClientToServer)]
public sealed partial class Client07ED
{
    [UInt(2)] public byte Unknown0 { get; private set; }
    [UInt(32)] public uint Unknown1 { get; private set; }
    [UInt(16)] public ushort Unknown2 { get; private set; }
    [UInt(14)] public ushort Unknown3 { get; private set; }
}
