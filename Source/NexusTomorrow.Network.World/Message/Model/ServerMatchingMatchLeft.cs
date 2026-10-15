using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerMatchingMatchLeft, Direction.ServerToClient)]
public sealed partial class ServerMatchingMatchLeft
{
    [UInt(5)] public Game.Static.Matching.MatchType Type { get; set; } // Not used by client, but server did fill this
}
