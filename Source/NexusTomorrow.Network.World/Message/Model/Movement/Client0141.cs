using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Movement;

[WireMessage(GameMessageOpcode.Client0141, Direction.ClientToServer)]
public sealed partial class Client0141
{
    [UInt(14)] public ushort Unknown0 { get; private set; }
    [UInt(32)] public uint Unknown1 { get; private set; }
}
