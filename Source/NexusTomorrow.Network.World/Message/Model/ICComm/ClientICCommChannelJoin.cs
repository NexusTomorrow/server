using NexusTomorrow.Game.Static.ICComm;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.ICComm;

[WireMessage(GameMessageOpcode.ClientICCommChannelJoin, Direction.ClientToServer)]
public sealed partial class ClientICCommChannelJoin
{
    [UInt(3)] public ICCommChannelType Type { get; private set; }
    [UInt(64)] public ulong GuildId { get; private set; } // only sent if Type is Guild
    [WideString] public string Name { get; private set; }
}
