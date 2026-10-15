using NexusTomorrow.Game.Static.Costume;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[WireMessage(GameMessageOpcode.ServerCostumeSaveResult, Direction.ServerToClient)]
public sealed partial class ServerCostumeSave
{
    [UInt(32)] public int Index { get; set; }
    [UInt(2)] public CostumeType Type { get; set; }
    [UInt(4)] public CostumeSaveResult Result { get; set; }
}
