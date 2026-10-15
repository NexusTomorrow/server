using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientStatisticsWindowOpen, Direction.ClientToServer)]
public sealed partial class ClientStatisticsWindowOpen
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
}
