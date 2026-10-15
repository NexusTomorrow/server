using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ClientGuildBankTransaction)]
[WireMessage(GameMessageOpcode.ClientGuildBankTransaction, Direction.ClientToServer)]
public sealed partial class ClientGuildBankTransaction : IReadable
{
    public Identity GuildIdentity { get; private set; } = new Identity();
    [UInt(64)] public ulong SourceItemGuid { get; private set; }
    [UInt(64)] public ulong DestinationItemGuid { get; private set; } // Sent if bag slot already contains an item, otherwise 0
    public ItemLocation Source { get; private set; } = new ItemLocation();
    public ItemLocation Destination { get; private set; } = new ItemLocation();
    [UInt(32)] public uint Count { get; private set; } // Value of 0 transfers entire stack

    public void Read(GamePacketReader reader)
    {
        GuildIdentity.Read(reader);
        SourceItemGuid = reader.ReadULong();
        DestinationItemGuid = reader.ReadULong();
        Source.Read(reader);
        Destination.Read(reader);
        Count = reader.ReadUInt();
    }
}
