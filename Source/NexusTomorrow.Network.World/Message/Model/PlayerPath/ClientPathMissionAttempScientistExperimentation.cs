using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.ClientPathMissionAttemptScientistExperimentation, Direction.ClientToServer)]
public sealed partial class ClientPathMissionAttemptScientistExperimentation
{
    [FixedArray(4), UInt(32)] public uint[] Choices { get; private set; } = new uint[4]; // Are all PathScientistExperimentationPatternDataId
}
