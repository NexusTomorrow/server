using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

/// <summary>
/// Commits a change to the character's appearance: the request the CommitCustomizationChanges confirm action sends.
/// </summary>
/// <remarks>
/// The confirm action prices the change and sends it only when the player can pay.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0144, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class CharacterCustomizationCommit
{
    /// <summary>
    /// The race chosen.
    /// </summary>
    [UInt(5)] public Race Race { get; private set; }

    /// <summary>
    /// The sex chosen; the client sets <see cref="Sex.Male"/> for a Chua.
    /// </summary>
    [UInt(2)] public Sex Sex { get; private set; }

    /// <summary>
    /// How many entries <see cref="LabelIds"/> and <see cref="LabelValues"/> hold.
    /// </summary>
    [UInt(32), Count] public uint LabelCount { get; private set; }

    /// <summary>
    /// Each customization slider set, by CharacterCustomizationLabel id.
    /// </summary>
    [Array(CountFrom = nameof(LabelCount)), UInt(32)] public List<uint> LabelIds { get; private set; } = [];

    /// <summary>
    /// Each slider's value, in <see cref="LabelIds"/>' order; the client checks that the label offers the value.
    /// </summary>
    [Array(CountFrom = nameof(LabelCount)), UInt(32)] public List<uint> LabelValues { get; private set; } = [];

    [UInt(32), Count] public uint Unknown5 { get; private set; }
    [Array(CountFrom = nameof(Unknown5)), Float32] public List<float> Unknown6 { get; private set; } = [];

    /// <summary>
    /// True to pay with service tokens, false to pay credits.
    /// </summary>
    [UInt(1)] public bool UseServiceTokens { get; private set; }
}
