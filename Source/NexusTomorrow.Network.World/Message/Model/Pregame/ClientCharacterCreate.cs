using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[WireMessage(GameMessageOpcode.ClientCharacterCreate, Direction.ClientToServer)]
public sealed partial class ClientCharacterCreate
{
    [UInt(32)] public uint CharacterCreationId { get; private set; }
    [WideString] public string Name { get; private set; }
    [UInt(3)] public byte Path { get; private set; }
    [UInt(32), Count] public uint LabelsCount { get; set; }
    [Array(CountFrom = nameof(LabelsCount)), UInt(32)] public List<uint> Labels { get; } = [];
    [Array(CountFrom = nameof(LabelsCount)), UInt(32)] public List<uint> Values { get; } = [];
    [Array(CountBits = 32), Float32] public List<float> Bones { get; } = [];
}
