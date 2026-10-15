using NexusTomorrow.Game.Static.Story;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Story.Message;

[WireStruct]
[UnionCase(StoryTextSourceType.Creature)]
public sealed partial class CreatureActor : Actor
{
    [UInt(18)] public uint Creature2Id { get; set; }
    [UInt(32)] public new uint TokenReplacementValue { get => base.TokenReplacementValue; set => base.TokenReplacementValue = value; }
    [NarrowString] public new string TokenName { get => base.TokenName; set => base.TokenName = value; }
}
