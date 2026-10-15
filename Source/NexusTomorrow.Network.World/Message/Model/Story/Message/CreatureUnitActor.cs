using NexusTomorrow.Game.Static.Story;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Story.Message;

[WireStruct]
[UnionCase(StoryTextSourceType.CreatureUnit)]
public sealed partial class CreatureUnitActor : Actor
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(18)] public uint Creature2Id { get; set; } // used if unit details have not been loaded on client
    [UInt(32)] public new uint TokenReplacementValue { get => base.TokenReplacementValue; set => base.TokenReplacementValue = value; }
    [NarrowString] public new string TokenName { get => base.TokenName; set => base.TokenName = value; }
}
