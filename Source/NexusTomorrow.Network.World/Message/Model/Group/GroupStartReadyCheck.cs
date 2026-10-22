using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Starts a ready check: the request GroupLib.ReadyCheck sends.
/// </summary>
/// <remarks>
/// The client sends it only while no ready check is running.
/// </remarks>
[WireMessage(GameMessageOpcode.Client044A, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class GroupStartReadyCheck
{
    /// <summary>
    /// The player's current group, by id.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }

    /// <summary>
    /// The script call's message.
    /// </summary>
    [WideString] public string Message { get; set; } = "";
}
