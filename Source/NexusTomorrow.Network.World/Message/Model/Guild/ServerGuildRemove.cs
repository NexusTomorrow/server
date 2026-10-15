using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ServerGuildRemove)]
[WireMessage(GameMessageOpcode.ServerGuildRemove, Direction.ServerToClient)]
public sealed partial class ServerGuildRemove : IWritable
{
    public Identity GuildIdentity { get; set; }

    public void Write(GamePacketWriter writer)
    {
        GuildIdentity.Write(writer);
    }
}
