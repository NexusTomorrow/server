using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathScientistAddFieldStudyToCreature)]
[WireMessage(GameMessageOpcode.ServerPathScientistAddFieldStudyToCreature, Direction.ServerToClient)]
public sealed partial class ServerPathScientistAddFieldStudyToCreature : IWritable
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(14)] public uint PathScientistFieldStudyId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(PathScientistFieldStudyId, 14);
    }
}
