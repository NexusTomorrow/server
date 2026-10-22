using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Sets the residence's privacy level: the request Residence:SetResidencePrivacyLevel sends, among others.
/// </summary>
/// <remarks>
/// It names no residence.
/// </remarks>
[WireMessage(GameMessageOpcode.Client00C9, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class HousingSetResidencePrivacyLevel
{
    /// <summary>
    /// A HousingLib.ResidencePrivacyLevel value: the script call's second argument; the client's other senders send 0 (Public)
    /// and 3 (Private).
    /// </summary>
    [UInt(32)] public uint PrivacyLevel { get; private set; }
}
