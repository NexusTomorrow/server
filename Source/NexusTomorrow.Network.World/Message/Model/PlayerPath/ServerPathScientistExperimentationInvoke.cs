using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathScientistExperimentationInvoke)]
[WireMessage(GameMessageOpcode.ServerPathScientistExperimentationInvoke, Direction.ServerToClient)]
public sealed partial class ServerPathScientistExperimentationInvoke : IWritable
{
    [UInt(32)] public uint UnitId { get; set; }
    [FixedArray(4), UInt(32)] public uint[] CurrentPattern { get; set; } = new uint[4]; // PathScientistExperimentationPatternDataId

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        for (int i = 0; i < 4; i++)
        {
            writer.Write(CurrentPattern[i]);
        }
    }
}
