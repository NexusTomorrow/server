using NexusTomorrow.Game.Static.Guild;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ServerUnitGuildNameplateChanged)]
[WireMessage(GameMessageOpcode.ServerUnitGuildNameplateChanged, Direction.ServerToClient)]
public sealed partial class ServerUnitGuildNameplateChanged : IWritable
{
    [UInt(32)] public uint UnitId { get; set; }
    [WideString] public string GuildName { get; set; }
    [UInt(4)] public GuildType GuildType { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.WriteStringWide(GuildName);
        writer.Write(GuildType, 4u);
    }
}
