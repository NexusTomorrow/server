using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// The result of redeeming CREDD; the client raises CREDDRedeemResult with it, in the world or on the character screen.
/// </summary>
[WireMessage(GameMessageOpcode.Server097B, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CreddRedeemResult
{
    /// <summary>
    /// The result, a PreGameLib.CodeEnumCharacterModifyResults code; its add-ons compare it with RedeemCREDDOk.
    /// </summary>
    [UInt(6)] public byte Result { get; set; }
}
