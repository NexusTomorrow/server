using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Invokes the settler build at a hub's unit, with its improvements.
/// </summary>
/// <remarks>
/// The client takes it only while the player is interacting as <see cref="InteractionType.SettlerImprovement"/>. It raises
/// InvokeSettlerBuild with the unit and the improvements its ids find.
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
    /// The improvements, by id, which the client passes to InvokeSettlerBuild.
    /// </summary>
    [Array(CountBits = 32), UInt(32)] public List<uint> ImprovementIds { get; set; } = [];
}
