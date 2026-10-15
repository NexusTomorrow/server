using NexusTomorrow.Game.Static.Costume;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[WireMessage(GameMessageOpcode.ServerCostumeItemUnlock, Direction.ServerToClient)]
public sealed partial class ServerCostumeItemUnlock
{
    [UInt(18)] public uint Item2Id { get; set; }
    [UInt(32)] public CostumeUnlockResult Result { get; set; }
}
