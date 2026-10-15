using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Reputation;
using NexusTomorrow.Game.Static.Story;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Story.Message;

[WireStruct]
[UnionCase(StoryTextSourceType.Player)]
public sealed partial class PlayerActor : Actor
{
    [UInt(32)] public uint UnitId { get; set; }
    [WideString] public string Name { get; set; }
    [UInt(32)] public uint Level { get; set; }
    [UInt(2)] public Sex Gender { get; set; }
    [UInt(5)] public Race Race { get; set; }
    [UInt(5)] public Class Class { get; set; }
    [UInt(14)] public Faction Faction { get; set; }
    [UInt(3)] public Game.Static.PlayerPath.Path Path { get; set; }
    [UInt(14)] public ushort TitleId { get; set; }
    [UInt(32)] public new uint TokenReplacementValue { get => base.TokenReplacementValue; set => base.TokenReplacementValue = value; }
    [NarrowString] public new string TokenName { get => base.TokenName; set => base.TokenName = value; }
}
