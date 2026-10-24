using NexusTomorrow.Game.Static.AccountInventory;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Reports the result of an account operation; the client raises AccountOperationResults with both values.
/// </summary>
[WireMessage(GameMessageOpcode.Server0970, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class AccountOperationResponse
{
    /// <summary>
    /// The operation.
    /// </summary>
    [UInt(32)] public AccountOperation Operation { get; set; }

    /// <summary>
    /// The result.
    /// </summary>
    [UInt(32)] public AccountOperationResult Result { get; set; }
}
