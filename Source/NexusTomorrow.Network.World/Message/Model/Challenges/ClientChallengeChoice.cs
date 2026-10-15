using NexusTomorrow.Game.Static.Challenges;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Challenges;

[Message(GameMessageOpcode.ClientChallengeChoice)]
[WireMessage(GameMessageOpcode.ClientChallengeChoice, Direction.ClientToServer)]
public sealed partial class ClientChallengeChoice : IReadable
{
    [UInt(14)] public ushort ChallengeId { get; private set; }
    [UInt(5)] public ChallengeChoice Choice { get; private set; }
    [UInt(32)] public uint Unused { get; private set; }

    public void Read(GamePacketReader reader)
    {
        ChallengeId = reader.ReadUShort(14u);
        Choice = reader.ReadEnum<ChallengeChoice>(5u);
        Unused = reader.ReadUInt();
    }
}
