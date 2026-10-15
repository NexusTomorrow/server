using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.ServerCharacterCreate, Direction.ServerToClient)]
public sealed partial class ServerCharacterCreate
{
    [UInt(64)] public ulong CharacterId { get; set; }
    [UInt(32)] public uint WorldId { get; set; }
    [UInt(3)] public CharacterModifyResult Result { get; set; }
}
