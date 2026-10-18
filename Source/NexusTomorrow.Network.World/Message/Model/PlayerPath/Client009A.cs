using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Client009A, Direction.ClientToServer)]
public sealed partial class Client009A
{
    [UInt(32)] public uint Unknown0 { get; private set; }
    [UInt(16)] public ushort Unknown1 { get; private set; }
    [UInt(32)] public uint Unknown2 { get; private set; }
    [UInt(1)] public bool Unknown3 { get; private set; }
}
