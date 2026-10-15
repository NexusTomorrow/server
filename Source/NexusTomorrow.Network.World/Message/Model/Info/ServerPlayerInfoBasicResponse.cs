using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

[Message(GameMessageOpcode.ServerPlayerInfoBasicResponse)]
[WireMessage(GameMessageOpcode.ServerPlayerInfoBasicResponse, Direction.ServerToClient)]
public sealed partial class ServerPlayerInfoBasicResponse : IWritable
{
    public PlayerInfoBase BaseData { get; set; }

    public void Write(GamePacketWriter writer)
    {
        BaseData.Write(writer);
    }
}
