using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Invokes the settler build at a hub's unit, with the improvements it offers.
/// </summary>
/// <remarks>
/// The client raises InvokeSettlerBuild with the unit and the improvements its ids find.
/// </remarks>
[WireMessage(GameMessageOpcode.Server066D, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SettlerBuildInvoke
{
    /// <summary>
    /// The hub's unit, by unit id.
    /// </summary>
    [UInt(32)] public uint HubUnitId { get; set; }

    /// <summary>
    /// The improvements on offer, by id.
    /// </summary>
    [Array(CountBits = 32), UInt(32)] public List<uint> ImprovementIds { get; set; } = [];
}
