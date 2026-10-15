using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

// Activates the guild nameplate for the player. This can be the nameplate
// of any guild the player is a member of.
[Message(GameMessageOpcode.ServerGuildNameplateChange)]
[WireMessage(GameMessageOpcode.ServerGuildNameplateChange, Direction.ServerToClient)]
public sealed partial class ServerGuildNameplateChange : IWritable
{
    public Identity GuildIdentity { get; set; }

    public void Write(GamePacketWriter writer)
    {
        GuildIdentity.Write(writer);
    }
}
