using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ClientGuildPerkActivate, Direction.ClientToServer)]
public sealed partial class ClientGuildPerkActivate : ClientGuildOperation
{
    // This is the same packet structure as ClientGuildOperation (0x4B1) however this message
    // is only sent for activating guild perks.

    // The Data field is the GuildPerkId of the perk being activated.
}
