using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// The answer to <see cref="CharacterRename"/>. On success the client renames the listed character.
/// </summary>
[WireMessage(GameMessageOpcode.Server014B, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CharacterRenameResult
{
    [UInt(6)] public byte Unknown0 { get; set; }
    [UInt(64)] public ulong Unknown1 { get; set; }
    [WideString] public string Unknown2 { get; set; } = "";
}
