using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathSoliderWhackAMoleBurrows)]
public class ServerPathSoldierWhackAMoleBurrows : IWritable
{
    public uint DelayUntilPop { get; set; }
    public uint[] UnitIds { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(DelayUntilPop);
        writer.Write(UnitIds.Length);
        foreach (uint unitId in UnitIds)
        {
            writer.Write(unitId);
        }
    }
}
