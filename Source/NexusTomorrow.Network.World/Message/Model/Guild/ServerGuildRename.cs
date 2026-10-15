using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ServerGuildRename)]
[WireMessage(GameMessageOpcode.ServerGuildRename, Direction.ServerToClient)]
public sealed partial class ServerGuildRename : IWritable
{
    public Identity GuildIdentity { get; set; }
    [WideString] public string Name { get; set; }

    public void Write(GamePacketWriter writer)
    {
        GuildIdentity.Write(writer);
        writer.WriteStringWide(Name);
    }
}
