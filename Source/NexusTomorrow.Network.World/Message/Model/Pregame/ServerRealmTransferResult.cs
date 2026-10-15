using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

// Client only processes message when on CharacterSelect screen.
[WireMessage(GameMessageOpcode.ServerRealmTransferResult, Direction.ServerToClient)]
public sealed partial class ServerRealmTransferResult
{
    [UInt(6)] public CharacterModifyResult Result { get; set; }
}
