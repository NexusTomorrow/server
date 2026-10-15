using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ServerGuildPerkDeactivated)]
[WireMessage(GameMessageOpcode.ServerGuildPerkDeactivated, Direction.ServerToClient)]
public sealed partial class ServerGuildPerkDeactivated : IWritable
{
    public Identity GuildIdentity { get; set; }
    [UInt(14)] public ushort GuildPerkId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        GuildIdentity.Write(writer);
        writer.Write(GuildPerkId, 14u);
    }
}
