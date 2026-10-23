using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Updates a residence's ownership state.
/// </summary>
/// <remarks>
/// For a value of 1 or 2 the client raises HousingOwnershipRecieved; for any other it closes the ownership panel when the
/// residence is the current one.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0533, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class HousingOwnership
{
    /// <summary>
    /// The residence, by id.
    /// </summary>
    public RealmScopedId ResidenceId { get; set; } = new();

    [UInt(32)] public uint Unknown1 { get; set; }
}
