using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ServerGuildMotdUpdate)]
[WireMessage(GameMessageOpcode.ServerGuildMotdUpdate, Direction.ServerToClient)]
public sealed partial class ServerGuildMotdUpdate : IWritable
{
    public Identity GuildIdentity { get; set; }
    [WideString] public string MessageOfTheDay { get; set; }

    public void Write(GamePacketWriter writer)
    {
        GuildIdentity.Write(writer);
        writer.WriteStringWide(MessageOfTheDay);
    }
}
