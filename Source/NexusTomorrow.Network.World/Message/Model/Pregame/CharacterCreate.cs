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
    [UInt(32)] public uint CreationId { get; set; }

    /// <summary>
    /// The new character's name.
    /// </summary>
    [WideString] public string Name { get; set; } = "";

    /// <summary>
    /// The character's path: 0 Soldier, 1 Settler, 2 Scientist, 3 Explorer.
    /// </summary>
    [UInt(3)] public byte Path { get; set; }

    /// <summary>
    /// How many look entries follow.
    /// </summary>
    [UInt(32), Count] public uint LookCount { get; set; }

    /// <summary>
    /// The look options the player set, by the ids the client keys its look options by.
    /// </summary>
    [Array(CountFrom = nameof(LookCount)), UInt(32)] public List<uint> LookIds { get; } = [];

    /// <summary>
    /// The value chosen for each option in <see cref="LookIds"/>.
    /// </summary>
    [Array(CountFrom = nameof(LookCount)), UInt(32)] public List<uint> LookValues { get; } = [];

    /// <summary>
    /// Values the client's model produces from the player's bone settings.
    /// </summary>
    [Array(CountBits = 32), Float32] public List<float> Bones { get; } = [];
}
