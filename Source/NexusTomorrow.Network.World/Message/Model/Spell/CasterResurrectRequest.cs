using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Offers the dead player a resurrection from a caster.
/// </summary>
/// <remarks>
/// The client marks the offer in the player's death state and raises CasterResurrectedPlayer with the caster's name.
/// </remarks>
[WireMessage(GameMessageOpcode.Server015B, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CasterResurrectRequest
{
    /// <summary>
    /// The caster, by unit id.
    /// </summary>
    [UInt(32)] public uint CasterUnitId { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(18)] public uint Unknown1 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown2 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [Float32] public float Unknown3 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [Float32] public float Unknown4 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [Float32] public Vector3 Unknown5 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(1)] public byte Unknown6 { get; set; }
}
