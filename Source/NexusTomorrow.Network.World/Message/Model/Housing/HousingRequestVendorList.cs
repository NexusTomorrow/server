using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Asks for a residence's vendor list: the request HousingLib.RequestVendorList sends, among others.
/// </summary>
[WireMessage(GameMessageOpcode.Client0525, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class HousingRequestVendorList
{
    /// <summary>
    /// The residence: from HousingLib.RequestVendorList, the player's current one.
    /// </summary>
    public RealmScopedId ResidenceId { get; private set; } = new();

    [UInt(2)] public byte Unknown1 { get; private set; }
}
