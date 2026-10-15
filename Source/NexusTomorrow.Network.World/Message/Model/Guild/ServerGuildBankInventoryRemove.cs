using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ServerGuildBankInventoryRemove, Direction.ServerToClient)]
public sealed partial class ServerGuildBankInventoryRemove
{
    public Identity GuildIdentity { get; set; }
    [UInt(64)] public ulong ItemGuid { get; private set; }
}
