using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Contract;

[WireMessage(GameMessageOpcode.Client035D, Direction.ClientToServer)]
public sealed partial class Client035D
{
    [UInt(15)] public ushort Unknown0 { get; private set; }
    [UInt(15)] public ushort Unknown1 { get; private set; }
    [UInt(1)] public bool Unknown2 { get; private set; }
}
