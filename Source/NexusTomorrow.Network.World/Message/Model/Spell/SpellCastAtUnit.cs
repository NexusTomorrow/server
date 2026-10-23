using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Casts a spell on its primary target.
/// </summary>
/// <remarks>
/// The client starts the cast itself; the server answers by its id with <see cref="SpellConfirmCast"/> or
/// <see cref="SpellCastResult"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Client009A, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class SpellCastAtUnit
{
    /// <summary>
    /// The cast, by the client's own id for it.
    /// </summary>
    [UInt(32)] public uint ClientCastId { get; private set; }

    [UInt(16)] public ushort Unknown1 { get; private set; }

    /// <summary>
    /// The primary target, by unit id.
    /// </summary>
    [UInt(32)] public uint TargetUnitId { get; private set; }

    [UInt(1)] public bool Unknown3 { get; private set; }
}
