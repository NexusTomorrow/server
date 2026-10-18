using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Client06E7, Direction.ClientToServer)]
public sealed partial class Client06E7
{
    [UInt(64)] public ulong Unknown0 { get; set; }
}
