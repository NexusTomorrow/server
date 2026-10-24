using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.Server00E7, Direction.ServerToClient)]
public sealed partial class Server00E7
{
    [UInt(64)] public ulong CharacterId { get; set; }

    [UInt(1)] public bool Unknown1 { get; set; }
}
