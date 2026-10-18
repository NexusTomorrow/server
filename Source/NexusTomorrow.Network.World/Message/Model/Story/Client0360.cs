using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story;

[WireMessage(GameMessageOpcode.Client0360, Direction.ClientToServer)]
public sealed partial class Client0360
{
    [UInt(15)] public uint Unknown0 { get; private set; }
    [UInt(1)] public bool Unknown1 { get; private set; }
}
