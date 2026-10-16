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
[Verified(Rung.Purpose)]
public sealed partial class CharacterCreate
{
    [UInt(32)] public uint Unknown0 { get; private set; }
    [WideString] public string Unknown1 { get; private set; } = "";
    [UInt(3)] public byte Unknown2 { get; private set; }
    [UInt(32), Count] public uint Unknown3 { get; set; }
    [Array(CountFrom = nameof(Unknown3)), UInt(32)] public List<uint> Unknown4 { get; } = [];
    [Array(CountFrom = nameof(Unknown3)), UInt(32)] public List<uint> Unknown5 { get; } = [];
    [Array(CountBits = 32), Float32] public List<float> Unknown6 { get; } = [];
}
