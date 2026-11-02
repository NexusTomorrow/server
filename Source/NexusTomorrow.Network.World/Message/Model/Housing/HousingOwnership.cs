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
[Verified(Rung.Fields)]
public sealed partial class HousingOwnership
{
    /// <summary>
    /// The residence, by id.
    /// </summary>
    [VerifiedMember] public RealmScopedId ResidenceId { get; set; } = new();

    /// <summary>
    /// The ownership state; 1 or 2 raises HousingOwnershipRecieved, any other closes the panel when it is the current residence.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Ownership { get; set; }
}
