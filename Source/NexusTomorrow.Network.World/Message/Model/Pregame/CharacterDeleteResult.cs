using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// The answer to <see cref="CharacterDelete"/>. A result of 0 removes the character from the client's list.
/// </summary>
[WireMessage(GameMessageOpcode.Server00E6, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CharacterDeleteResult
{
    [UInt(6)] public byte Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
}
