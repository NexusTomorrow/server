using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.ServerCharacterSelectFail, Direction.ServerToClient)]
public sealed partial class ServerCharacterSelectFail
{
    [UInt(3)] public CharacterSelectResult Result { get; set; }
}
