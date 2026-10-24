using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[WireMessage(GameMessageOpcode.Server025A, Direction.ServerToClient)]
public sealed partial class CostumeItemList
{
    [Array(CountBits = 32), UInt(32)] public List<uint> Unknown0 { get; set; } = [];
}
