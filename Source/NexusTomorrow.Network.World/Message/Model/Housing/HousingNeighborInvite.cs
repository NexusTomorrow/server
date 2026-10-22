using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Client0512, Direction.ClientToServer)]
public sealed partial class HousingNeighborInvite
{
    [UInt(14)] public ushort Unknown0 { get; set; }
    [UInt(64)] public ulong Unknown1 { get; set; }
    [WideString] public string Unknown2 { get; set; } = "";
}
