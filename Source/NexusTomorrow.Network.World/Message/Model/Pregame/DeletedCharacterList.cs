using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// The account's deleted characters, kept for the character-select interface to look up.
/// </summary>
/// <remarks>
/// Taken while character select is active; the client replaces the list it holds and raises nothing. No interface script looks the
/// list up.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0116, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
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

        [WideString] public string Unknown2 { get; set; } = "";
        [UInt(32)] public uint Unknown3 { get; set; }
        [UInt(14)] public ushort Unknown4 { get; set; }
        [UInt(14)] public ushort Unknown5 { get; set; }
        [UInt(2)] public ushort Unknown6 { get; set; }
        [UInt(14)] public ushort Unknown7 { get; set; }
        [UInt(3)] public ushort Unknown8 { get; set; }
        [UInt(15)] public ushort Unknown9 { get; set; }
        [UInt(15)] public ushort Unknown10 { get; set; }
        [Float32] public float Unknown11 { get; set; }
    }

    /// <summary>
    /// The deleted characters.
    /// </summary>
    [Array(CountBits = 32)] public List<DeletedCharacter> Characters { get; set; } = [];
}
