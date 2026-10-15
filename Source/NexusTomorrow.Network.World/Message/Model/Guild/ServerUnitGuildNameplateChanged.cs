using NexusTomorrow.Game.Static.Guild;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ServerUnitGuildNameplateChanged, Direction.ServerToClient)]
public sealed partial class ServerUnitGuildNameplateChanged
{
    [UInt(32)] public uint UnitId { get; set; }
    [WideString] public string GuildName { get; set; }
    [UInt(4)] public GuildType GuildType { get; set; }
}
