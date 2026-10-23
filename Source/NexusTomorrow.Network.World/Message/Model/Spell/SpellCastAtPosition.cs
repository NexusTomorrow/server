using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Casts a spell at the ground point.
/// </summary>
/// <remarks>
/// The client sends it in place of <see cref="SpellCastAtUnit"/> when the spell's data asks for it. The client starts the cast itself; the
/// server answers by its id with <see cref="SpellConfirmCast"/> or <see cref="SpellCastResult"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Client009B, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class SpellCastAtPosition
{
    /// <summary>
    /// The cast, by the client's own id for it.
    /// </summary>
    [UInt(32)] public uint ClientCastId { get; private set; }

    [UInt(16)] public ushort Unknown1 { get; private set; }

    /// <summary>
    /// The ground point the cast aims at.
    /// </summary>
    public Client0096Part0 Position { get; private set; } = new Client0096Part0();
}
