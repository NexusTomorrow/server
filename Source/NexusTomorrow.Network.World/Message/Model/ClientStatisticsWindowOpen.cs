using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientStatisticsWindowOpen)]
[WireMessage(GameMessageOpcode.ClientStatisticsWindowOpen, Direction.ClientToServer)]
public sealed partial class ClientStatisticsWindowOpen : IReadable
{
    [UInt(6)] public uint Always35 { get; private set; }
    [UInt(10)] public uint WindowId { get; private set; }
    [UInt(64)] public ulong CharacterId { get; private set; }
    [UInt(64)] public ulong Always0_1 { get; private set; }
    [UInt(64)] public ulong TimeSpentOpen { get; private set; }
    [UInt(64)] public ulong Always0_2 { get; private set; }
    [UInt(64)] public ulong Always0_3 { get; private set; }
    [UInt(64)] public ulong Always0_4 { get; private set; }
    [WideString] public string WindowName { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Always35 = reader.ReadUInt(6);
        WindowId = reader.ReadUInt(0xA);
        CharacterId = reader.ReadULong();
        Always0_1 = reader.ReadULong();
        TimeSpentOpen = reader.ReadULong();
        Always0_2 = reader.ReadULong();
        Always0_3 = reader.ReadULong();
        Always0_4 = reader.ReadULong();
        WindowName = reader.ReadWideString();
    }
}
