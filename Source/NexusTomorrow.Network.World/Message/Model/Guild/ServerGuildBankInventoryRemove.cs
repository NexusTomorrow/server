using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ServerGuildBankInventoryRemove)]
[WireMessage(GameMessageOpcode.ServerGuildBankInventoryRemove, Direction.ServerToClient)]
public sealed partial class ServerGuildBankInventoryRemove : IWritable
{
    public Identity GuildIdentity { get; set; }
    [UInt(64)] public ulong ItemGuid { get; private set; }

    public void Write(GamePacketWriter writer)
    {
        GuildIdentity.Write(writer);
        writer.Write(ItemGuid);
    }
}
