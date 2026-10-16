using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// The account's deleted characters, kept for the character-select interface to look up.
/// </summary>
/// <remarks>
/// The client replaces the list it holds, in any state, and raises nothing.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0116, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class DeletedCharacterList
{
    /// <summary>
    /// One deleted character.
    /// </summary>
    [WireStruct]
    public sealed partial class DeletedCharacter
    {
        /// <summary>
        /// The client gives this value no meaning.
        /// </summary>
        [UInt(64)] public ulong Unknown0 { get; set; }

        /// <summary>
        /// The client gives this value no meaning.
        /// </summary>
        [UInt(32)] public uint Unknown1 { get; set; }

        /// <summary>
        /// The character's name.
        /// </summary>
        [WideString] public string Name { get; set; } = "";

        /// <summary>
        /// The character's level.
        /// </summary>
        [UInt(32)] public uint Level { get; set; }

        /// <summary>
        /// The character's class id.
        /// </summary>
        [UInt(14)] public ushort ClassId { get; set; }

        /// <summary>
        /// The character's race id.
        /// </summary>
        [UInt(14)] public ushort RaceId { get; set; }

        /// <summary>
        /// The character's gender id.
        /// </summary>
        [UInt(2)] public ushort GenderId { get; set; }

        /// <summary>
        /// The character's faction id.
        /// </summary>
        [UInt(14)] public ushort FactionId { get; set; }

        /// <summary>
        /// The character's path id.
        /// </summary>
        [UInt(3)] public ushort PathId { get; set; }

        /// <summary>
        /// The world the character was in.
        /// </summary>
        [UInt(15)] public ushort WorldId { get; set; }

        /// <summary>
        /// The zone the character was in, as a world zone id.
        /// </summary>
        [UInt(15)] public ushort WorldZoneId { get; set; }

        /// <summary>
        /// The client gives this value no meaning. It hands it to its interface as the character's purge date, which no script reads.
        /// </summary>
        [Float32] public float Unknown11 { get; set; }
    }

    /// <summary>
    /// The deleted characters.
    /// </summary>
    [Array(CountBits = 32)] public List<DeletedCharacter> Characters { get; set; } = [];
}
