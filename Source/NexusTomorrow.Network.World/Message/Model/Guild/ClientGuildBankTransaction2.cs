using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ClientGuildBankTransaction2, Direction.ClientToServer)]
public sealed partial class ClientGuildBankTransaction2
{
    public Identity GuildIdentity { get; private set; } = new Identity();
    [UInt(64)] public ulong SourceItemGuid { get; private set; }
    [UInt(64)] public ulong DestinationItemGuid { get; private set; } // Sent if bag slot already contains an item, otherwise 0
    [UInt(18)] public uint SourceItem2Id { get; private set; }
    public ItemLocation Destination { get; private set; } = new ItemLocation();
    public ItemLocation Source { get; private set; } = new ItemLocation();
    [UInt(1)] public bool UseFirstFreeSlot { get; private set; }
    [UInt(32)] public uint Count { get; private set; } // Value of 0 transfers entire stack
}
