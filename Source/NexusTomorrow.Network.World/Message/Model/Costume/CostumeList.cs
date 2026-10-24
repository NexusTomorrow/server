using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[WireMessage(GameMessageOpcode.Server00D9, Direction.ServerToClient)]
public sealed partial class CostumeList
{
    [Array(CountBits = 32)] public List<CostumeEntry> Unknown0 { get; set; } = [];
}
