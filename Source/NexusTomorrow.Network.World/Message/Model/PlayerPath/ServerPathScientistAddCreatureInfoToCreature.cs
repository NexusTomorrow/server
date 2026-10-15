using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathScientistAddCreatureInfoToCreature)]
[WireMessage(GameMessageOpcode.ServerPathScientistAddCreatureInfoToCreature, Direction.ServerToClient)]
public sealed partial class ServerPathScientistAddCreatureInfoToCreature : IWritable
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(14)] public uint PathScientistCreatureInfoId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(PathScientistCreatureInfoId, 14);
    }
}
