using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

[WireMessage(GameMessageOpcode.Server0106, Direction.ServerToClient)]
public sealed partial class GenericErrorNotify
{
    [UInt(8)] public byte Unknown0 { get; set; }
}
