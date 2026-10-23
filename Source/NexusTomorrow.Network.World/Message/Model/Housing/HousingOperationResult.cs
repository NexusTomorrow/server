using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Reports the result of a housing operation.
/// </summary>
/// <remarks>
/// The client raises HousingResult with the name and the result; for results 12, 13 and 44 it also raises
/// HousingNeighborInviteDeclined with the name, and for 16 HousingNeighborInviteAccepted.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0536, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class HousingOperationResult
{
    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    public RealmScopedId Unknown0 { get; set; } = new();

    /// <summary>
    /// The player the result concerns, by name.
    /// </summary>
    [WideString] public string PlayerName { get; set; } = "";

    /// <summary>
    /// A HousingLib.HousingResult value.
    /// </summary>
    [UInt(7)] public byte Result { get; set; }
}
