using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

// Only used by client when on the CharacterSelect screen
[WireMessage(GameMessageOpcode.ServerCharacterRenameResult, Direction.ServerToClient)]
public sealed partial class ServerCharacterRenameResult
{
    [UInt(6)] public CharacterModifyResult Result { get; set; }
    [UInt(64)] public ulong CharacterId { get; set; }
    [WideString] public string Name { get; set; }
}
