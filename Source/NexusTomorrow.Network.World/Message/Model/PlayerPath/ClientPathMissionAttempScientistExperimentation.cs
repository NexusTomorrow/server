using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ClientPathMissionAttemptScientistExperimentation)]
[WireMessage(GameMessageOpcode.ClientPathMissionAttemptScientistExperimentation, Direction.ClientToServer)]
public sealed partial class ClientPathMissionAttemptScientistExperimentation : IReadable
{
    [FixedArray(4), UInt(32)] public uint[] Choices { get; private set; } = new uint[4]; // Are all PathScientistExperimentationPatternDataId

    public void Read(GamePacketReader reader)
    {
        for (uint i = 0u; i < 4; i++)
            Choices[i] = reader.ReadUInt();
    }
}
