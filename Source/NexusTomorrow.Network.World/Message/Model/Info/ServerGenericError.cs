using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Info;

[WireMessage(GameMessageOpcode.ServerGenericError, Direction.ServerToClient)]
public sealed partial class ServerGenericError
{
    [UInt(8)] public GenericError Error { get; set; }
}
