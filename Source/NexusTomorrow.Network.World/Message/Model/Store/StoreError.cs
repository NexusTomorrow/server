using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Store;

/// <summary>
/// Reports a store error; the client raises StoreError with the code.
/// </summary>
[WireMessage(GameMessageOpcode.Server098A, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class StoreError
{
    /// <summary>
    /// The store error code, fired as the StoreError event's argument.
    /// </summary>
    [VerifiedMember, UInt(5)] public byte ErrorCode { get; set; }
}
