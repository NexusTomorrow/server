using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathSettlerBuildResult)]
[WireMessage(GameMessageOpcode.ServerPathSettlerBuildResult, Direction.ServerToClient)]
public sealed partial class ServerPathSettlerBuildResult : IWritable
{
    [UInt(32)] public uint Result { get; set; }  // Is an enum but was never used in client. Passed to SetterBuildResult lua event.

    [UInt(15)] public uint Unknown { get; set; } // If this is not zero, client gets localizedTextId from PathSettlerImproveGroup
                                                 // tbl entry to pass name of improvement that was build to SetterBuildResult event.
    [UInt(14)] public uint PathSettlerImprovementGroupId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Result);
        writer.Write(Unknown, 15);
        writer.Write(PathSettlerImprovementGroupId, 14);
    }
}
