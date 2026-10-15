using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[Message(GameMessageOpcode.ClientCharacterCreate)]
[WireMessage(GameMessageOpcode.ClientCharacterCreate, Direction.ClientToServer)]
public sealed partial class ClientCharacterCreate : IReadable
{
    [UInt(32)] public uint CharacterCreationId { get; private set; }
    [WideString] public string Name { get; private set; }
    [UInt(3)] public byte Path { get; private set; }
    [UInt(32), Count] public uint LabelsCount { get; set; }
    [Array(CountFrom = nameof(LabelsCount)), UInt(32)] public List<uint> Labels { get; } = [];
    [Array(CountFrom = nameof(LabelsCount)), UInt(32)] public List<uint> Values { get; } = [];
    [Array(CountBits = 32), Float32] public List<float> Bones { get; } = [];

    public void Read(GamePacketReader reader)
    {
        CharacterCreationId = reader.ReadUInt();
        Name = reader.ReadWideString();
        Path = reader.ReadByte(3);

        uint customisationCount = reader.ReadUInt();
        for (uint i = 0u; i < customisationCount; i++)
            Labels.Add(reader.ReadUInt());
        for (uint i = 0u; i < customisationCount; i++)
            Values.Add(reader.ReadUInt());

        uint boneCount = reader.ReadUInt();
        for (uint i = 0u; i < boneCount; i++)
            Bones.Add(reader.ReadSingle());
    }
}
