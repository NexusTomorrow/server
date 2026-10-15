using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ClientPathMissionAttemptScientistExperimentation)]
public class ClientPathMissionAttemptScientistExperimentation : IReadable
{
    public List<uint> Choices { get; private set; } = []; // Are all PathScientistExperimentationPatternDataId

    public void Read(GamePacketReader reader)
    {
        for (uint i = 0u; i < 4; i++)
            Choices.Add(reader.ReadUInt());
    }
}
