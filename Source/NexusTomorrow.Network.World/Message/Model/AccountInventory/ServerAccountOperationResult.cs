using NexusTomorrow.Game.Static.AccountInventory;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.ServerAccountOperationResult, Direction.ServerToClient)]
public sealed partial class ServerAccountOperationResult
{
    [UInt(32)] public AccountOperation Operation { get; set; }
    [UInt(32)] public AccountOperationResult Result { get; set; }
}
