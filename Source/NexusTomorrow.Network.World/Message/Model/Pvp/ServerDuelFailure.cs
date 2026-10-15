using NexusTomorrow.Game.Static.Pvp;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp;

[WireMessage(GameMessageOpcode.ServerDuelFailure, Direction.ServerToClient)]
public sealed partial class ServerDuelFailure
{
    [UInt(5)] public DuelFailureReason Reason { get; set; }
}
