using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Casts the bind spell on a unit that is a bind point.
/// </summary>
/// <remarks>
/// The client starts the cast itself; the server answers by its id with <see cref="SpellConfirmCast"/> or
/// <see cref="SpellCastResult"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0098, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class BindPointCast
{
    /// <summary>
    /// The cast, by the client's own id for it.
    /// </summary>
    [UInt(32)] public uint ClientCastId { get; private set; }

    /// <summary>
    /// The bind point, by unit id.
    /// </summary>
    [UInt(32)] public uint TargetUnitId { get; private set; }
}
