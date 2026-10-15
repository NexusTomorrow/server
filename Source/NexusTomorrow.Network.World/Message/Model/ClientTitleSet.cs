using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientTitleSet, Direction.ClientToServer)]
public sealed partial class ClientTitleSet
{
    [UInt(14)] public ushort TitleId { get; set; }
}
