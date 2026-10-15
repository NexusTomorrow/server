using NexusTomorrow.Game.Static.Costume;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[WireMessage(GameMessageOpcode.ServerCostumeItemUnlockMultiple, Direction.ServerToClient)]
public sealed partial class ServerCostumeItemUnlockMultiple
{
    [UInt(32)] public CostumeUnlockResult Result { get; set; }
    [Array(CountBits = 32), UInt(32)] public List<uint> Item2Ids { get; set; } = [];
}
