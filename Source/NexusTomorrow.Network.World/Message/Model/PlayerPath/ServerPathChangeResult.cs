using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;


[WireMessage(GameMessageOpcode.ServerPathChangeResult, Direction.ServerToClient)]
public sealed partial class ServerPathChangeResult
{
    [UInt(8)] public GenericError Result { get; set; }
}
