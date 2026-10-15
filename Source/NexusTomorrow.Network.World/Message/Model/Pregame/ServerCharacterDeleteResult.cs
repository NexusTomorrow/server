using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.ServerCharacterDeleteResult, Direction.ServerToClient)]
public sealed partial class ServerCharacterDeleteResult
{
    [UInt(6)] public CharacterModifyResult Result { get; set; }
    [UInt(32)] public uint Data { get; set; } // used to select type of guild to show in error when result = DeleteFailed_GuildMaster
}
