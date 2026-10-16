using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// Creates a character from a character creation option, a name and the player's choices.
/// </summary>
/// <remarks>
/// Sent inside a <see cref="Network.Message.Model.NestedMessage"/>, itself inside a
/// <see cref="Network.Message.Model.ClientEnvelope"/>; the client then waits for <see cref="CharacterCreateResult"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Client025B, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class CharacterCreate
{
    /// <summary>
    /// The character creation option, as an id in the CharacterCreation table.
    /// </summary>
    [UInt(32)] public uint CreationId { get; private set; }

    /// <summary>
    /// The new character's name.
    /// </summary>
    [WideString] public string Name { get; private set; } = "";

    /// <summary>
    /// The character's path: 0 soldier, 1 settler, 2 scientist, 3 explorer.
    /// </summary>
    [UInt(3)] public byte Path { get; private set; }

    /// <summary>
    /// How many look entries follow.
    /// </summary>
    [UInt(32), Count] public uint LookCount { get; set; }

    /// <summary>
    /// The look labels, as ids in the CharacterCustomizationLabel table.
    /// </summary>
    [Array(CountFrom = nameof(LookCount)), UInt(32)] public List<uint> LookLabels { get; } = [];

    /// <summary>
    /// The value chosen for each label in <see cref="LookLabels"/>.
    /// </summary>
    [Array(CountFrom = nameof(LookCount)), UInt(32)] public List<uint> LookValues { get; } = [];

    /// <summary>
    /// Values the client's model produces from the player's bone settings, one per bone component listed for the character's race
    /// and gender.
    /// </summary>
    [Array(CountBits = 32), Float32] public List<float> Bones { get; } = [];
}
