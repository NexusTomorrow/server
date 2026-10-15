using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientCharacterAppearanceChange, Direction.ClientToServer)]
public sealed partial class ClientCharacterAppearanceChange
{
    [UInt(5)] public Race Race { get; private set; } // 5
    [UInt(2)] public Sex Sex { get; private set; } // 2
    [UInt(32), Count] public uint CustomisationCount { get; private set; }
    [Array(CountFrom = nameof(CustomisationCount)), UInt(32)] public List<uint> Labels { get; private set; } = [];
    [Array(CountFrom = nameof(CustomisationCount)), UInt(32)] public List<uint> Values { get; private set; } = [];
    [UInt(32), Count] public uint BoneCount { get; private set; }
    [Array(CountFrom = nameof(BoneCount)), Float32] public List<float> Bones { get; private set; } = [];
    [UInt(1)] public bool UseServiceTokens { get; private set; }
}
