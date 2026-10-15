using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class PropertyValue : IWritable
{
    [UInt(8)] public Property Property { get; set; }
    [Float32] public float BaseValue { get; set; }
    [Float32] public float Value { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Property, 8);
        writer.Write(BaseValue);
        writer.Write(Value);
    }
}
