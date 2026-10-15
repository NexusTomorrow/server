using NexusTomorrow.Game.Static.Abilities;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[Message(GameMessageOpcode.ClientRespecAmps)]
[WireMessage(GameMessageOpcode.ClientRespecAmps, Direction.ClientToServer)]
public sealed partial class ClientRespecAmps : IReadable
{
    [UInt(3)] public byte SpecIndex { get; private set; }
    [UInt(3)] public AmpRespecType RespecType { get; private set; }
    [UInt(32)] public uint Value { get; private set; } // Is EldanAugmentationId for RespecType = Single
                                                       // Is EldanAugmentationCategoryId for RespecType = Section
                                                       // Is zero for RespecType = Full

    public void Read(GamePacketReader reader)
    {
        SpecIndex = reader.ReadByte(3u);
        RespecType = reader.ReadEnum<AmpRespecType>(3u);
        Value = reader.ReadUInt();
    }
}
