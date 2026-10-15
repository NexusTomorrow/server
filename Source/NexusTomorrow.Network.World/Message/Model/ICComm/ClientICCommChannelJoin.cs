using NexusTomorrow.Game.Static.ICComm;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.ICComm;

[Message(GameMessageOpcode.ClientICCommChannelJoin)]
[WireMessage(GameMessageOpcode.ClientICCommChannelJoin, Direction.ClientToServer)]
public sealed partial class ClientICCommChannelJoin : IReadable
{
    [UInt(3)] public ICCommChannelType Type { get; private set; }
    [UInt(64)] public ulong GuildId { get; private set; } // only sent if Type is Guild
    [WideString] public string Name { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Type = reader.ReadEnum<ICCommChannelType>(3u);
        GuildId = reader.ReadULong();
        Name = reader.ReadWideString();
    }
}
