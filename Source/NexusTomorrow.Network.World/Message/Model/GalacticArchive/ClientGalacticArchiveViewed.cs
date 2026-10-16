using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.GalacticArchive;

[WireMessage(GameMessageOpcode.Client0105, Direction.ClientToServer)]
public sealed partial class Client0105
{
    [UInt(14)] public uint Unknown0 { get; private set; }
}
