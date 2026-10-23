using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Shows a settler hub's reward: the client raises SettlerHubReward with the hub's reward text.
/// </summary>
[WireMessage(GameMessageOpcode.Server066F, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SettlerHubReward
{
    /// <summary>
    /// The hub, by id.
    /// </summary>
    [UInt(14)] public uint HubId { get; set; }
}
