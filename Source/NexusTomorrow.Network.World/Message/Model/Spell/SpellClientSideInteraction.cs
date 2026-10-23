using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Reports the player's side of an interaction a cast asks for.
/// </summary>
[WireMessage(GameMessageOpcode.Client0805, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class SpellClientSideInteraction
{
    /// <summary>
    /// The cast, by the server's id for it.
    /// </summary>
    [UInt(32)] public uint CastId { get; private set; }

    [UInt(3)] public byte Unknown1 { get; private set; }
    [UInt(32)] public uint Unknown2 { get; private set; }
}
