using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary> Updates stack count of item in guild bank inventory. </summary>
[Message(GameMessageOpcode.ServerGuildBankInventoryCount)]
[WireMessage(GameMessageOpcode.ServerGuildBankInventoryCount, Direction.ServerToClient)]
public sealed partial class ServerGuildBankInventoryCount : IWritable
{
    public Identity GuildIdentity { get; set; }
    [UInt(64)] public ulong ItemGuid { get; set; }
    [UInt(32)] public uint Count { get; set; }

    public void Write(GamePacketWriter writer)
    {
        GuildIdentity.Write(writer);
        writer.Write(ItemGuid);
        writer.Write(Count);
    }
}
