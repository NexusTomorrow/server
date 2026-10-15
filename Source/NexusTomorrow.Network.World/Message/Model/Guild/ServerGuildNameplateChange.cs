using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

// Activates the guild nameplate for the player. This can be the nameplate
// of any guild the player is a member of.
[WireMessage(GameMessageOpcode.ServerGuildNameplateChange, Direction.ServerToClient)]
public sealed partial class ServerGuildNameplateChange
{
    public Identity GuildIdentity { get; set; }
}
