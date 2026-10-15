using NexusTomorrow.Game.Static.Story;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story.Message;

[WireStruct]
public abstract partial class Actor : IWritable
{
    [NotOnWire] public virtual StoryTextSourceType Type { get; }

    // Each arm puts these two after its own fields, which a base's wire members cannot follow, so every arm
    // redeclares them over this storage in its own wire order.
    [NotOnWire] public uint TokenReplacementValue { get; set; }
    [NotOnWire] public string TokenName { get; set; } // used to specify a custom token name in a localized string i.e. $(direction) $(enemy_fac) $(resource_type)

    public virtual void Write(GamePacketWriter writer)
    {
        writer.Write(TokenReplacementValue);
        writer.WriteString(TokenName);
    }
}
