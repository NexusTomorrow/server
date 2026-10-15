using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerMatchingEligibilityChanged, Direction.ServerToClient)]
public sealed partial class ServerMatchingEligibilityChanged
{
    [UInt(32)] public uint MatchingEligibilityFlags { get; set; } // Checked against tbl matchingMapPrequisite->matchingEligibilityFlagEnum
}
