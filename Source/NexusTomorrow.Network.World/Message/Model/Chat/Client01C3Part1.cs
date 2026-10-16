using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Chat;
using NexusTomorrow.Network.World.Chat.Model;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireStruct]
public partial class Client01C3Part1
{
    [UInt(4)] public byte Unknown0 { get; set; }
    [UInt(16)] public ushort Unknown1 { get; set; }
    [UInt(16)] public ushort Unknown2 { get; set; }
    [Union(nameof(Unknown0))] public IClient01C3Union0 Unknown3 { get; set; } = new Client01C3Union0Arm00();
}
