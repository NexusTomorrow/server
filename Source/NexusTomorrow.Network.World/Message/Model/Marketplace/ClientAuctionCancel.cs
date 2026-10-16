using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.Client0094, Direction.ClientToServer)]
public sealed partial class Client0094
{
    [UInt(18)] public uint Unknown0 { get; private set; }
    [UInt(64)] public ulong Unknown1 { get; private set; }
}
