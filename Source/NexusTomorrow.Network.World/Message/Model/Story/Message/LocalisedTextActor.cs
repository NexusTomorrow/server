using NexusTomorrow.Game.Static.Story;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Story.Message;

[WireStruct]
[UnionCase(StoryTextSourceType.LocalizedText)]
public sealed partial class LocalisedTextActor : Actor
{
    [UInt(21)] public uint LocalisedTextId { get; set; }
    [UInt(32)] public new uint TokenReplacementValue { get => base.TokenReplacementValue; set => base.TokenReplacementValue = value; }
    [NarrowString] public new string TokenName { get => base.TokenName; set => base.TokenName = value; }
}
