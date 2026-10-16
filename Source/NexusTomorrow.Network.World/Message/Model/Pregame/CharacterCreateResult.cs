using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// The answer to <see cref="CharacterCreate"/>. On success the client selects the new character.
/// </summary>
[WireMessage(GameMessageOpcode.Server00DC, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CharacterCreateResult
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(6)] public byte Unknown2 { get; set; }
}
