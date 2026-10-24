using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

/// <summary>
/// Adds a value to the client's list of disabled gameplay systems, or removes it.
/// </summary>
/// <remarks>
/// GameLib.GetDisabledGameplaySystems returns the list, and the client raises DisabledGameplaySystemNotification.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0366, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class GameplaySystemDisabledChanged
{
    [UInt(3)] public byte Unknown0 { get; set; }

    /// <summary>
    /// The client gives this value no meaning. It discards it.
    /// </summary>
    [UInt(32)] public uint Unknown1 { get; set; }

    /// <summary>
    /// Set adds <see cref="Unknown0"/> to the client's disabled list, clear removes it.
    /// </summary>
    [UInt(1)] public bool Disabled { get; set; }
}
