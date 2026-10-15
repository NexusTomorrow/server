using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerMatchingEligibilityChanged)]
[WireMessage(GameMessageOpcode.ServerMatchingEligibilityChanged, Direction.ServerToClient)]
public sealed partial class ServerMatchingEligibilityChanged : IWritable
{
    [UInt(32)] public uint MatchingEligibilityFlags { get; set; } // Checked against tbl matchingMapPrequisite->matchingEligibilityFlagEnum

    public void Write(GamePacketWriter writer)
    {
        writer.Write(MatchingEligibilityFlags);
    }
}
