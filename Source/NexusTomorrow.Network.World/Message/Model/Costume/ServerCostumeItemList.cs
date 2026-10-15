using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[WireMessage(GameMessageOpcode.ServerCostumeItemList, Direction.ServerToClient)]
public sealed partial class ServerCostumeItemList
{
    [Array(CountBits = 32), UInt(32)] public List<uint> Item2Ids { get; set; } = [];
}
