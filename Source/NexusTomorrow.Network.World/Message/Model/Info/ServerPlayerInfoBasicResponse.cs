using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

[WireMessage(GameMessageOpcode.ServerPlayerInfoBasicResponse, Direction.ServerToClient)]
public sealed partial class ServerPlayerInfoBasicResponse
{
    public PlayerInfoBase BaseData { get; set; }
}
