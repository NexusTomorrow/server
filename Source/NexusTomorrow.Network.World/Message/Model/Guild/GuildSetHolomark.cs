using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Sets how the player's guild holomark shows: the request GameLib.ShowGuildHolomark sends.
/// </summary>
/// <remarks>
/// The shipped add-ons call GameLib.ShowGuildHolomark with four flags: whether the left, the right and the back holomark
/// show, then a fourth sent unchanged as DisplayNear. The client sends this request only when the setting changes.
/// </remarks>
[WireMessage(GameMessageOpcode.Client010C, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class GuildSetHolomark
{
    /// <summary>
    /// True to hide the left holomark: the call's left flag negated.
    /// </summary>
    [UInt(1)] public bool HideLeft { get; private set; }

    /// <summary>
    /// True to hide the right holomark: the call's right flag negated.
    /// </summary>
    [UInt(1)] public bool HideRight { get; private set; }

    /// <summary>
    /// True to hide the back holomark: the call's back flag negated.
    /// </summary>
    [UInt(1)] public bool HideBack { get; private set; }

    /// <summary>
    /// The call's fourth flag, unchanged.
    /// </summary>
    [UInt(1)] public bool DisplayNear { get; private set; }
}
