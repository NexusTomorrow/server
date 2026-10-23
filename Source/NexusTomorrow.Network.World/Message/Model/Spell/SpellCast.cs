using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Casts a spell on a target or at a point.
/// </summary>
/// <remarks>
/// The client starts the cast itself; the server answers by its id with <see cref="SpellConfirmCast"/> or
/// <see cref="SpellCastResult"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0096, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class SpellCast
{
    /// <summary>
    /// The cast, by the client's own id for it.
    /// </summary>
    [UInt(32)] public uint ClientCastId { get; private set; }

    [UInt(4)] public byte Unknown1 { get; private set; }
    [UInt(4)] public byte Unknown2 { get; private set; }

    /// <summary>
    /// The target, by unit id.
    /// </summary>
    [UInt(32)] public uint TargetUnitId { get; private set; }

    /// <summary>
    /// The point the cast aims at: the target's position, or the ground point.
    /// </summary>
    public Client0096Part0 Position { get; private set; } = new Client0096Part0();
}
