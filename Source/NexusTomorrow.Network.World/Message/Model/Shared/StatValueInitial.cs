using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class StatValueInitial : IWritable
{
    /// <summary>
    /// The stat's value in the encoding <see cref="Type"/> selects.
    /// </summary>
    public interface IPayload : IWireStruct
    {
    }

    [WireStruct]
    [UnionCase(StatType.Integer)]
    public sealed partial class IntegerValue : IPayload
    {
        [UInt(32)] public uint Value { get; set; }
    }

    [WireStruct]
    [UnionCase(StatType.Float)]
    public sealed partial class FloatValue : IPayload
    {
        [Float32] public float Value { get; set; }
    }

    [WireStruct]
    [UnionCase(StatType.Data)]
    public sealed partial class DataValue : IPayload
    {
        [UInt(32)] public uint Value { get; set; }
        [UInt(32)] public uint Data { get; set; }
    }

    [UInt(5)] public Stat Stat { get; set; }
    [UInt(2)] public StatType Type { get; set; }
    [Union(nameof(Type))] public IPayload Payload { get; set; } = new IntegerValue();
    [NotOnWire] public float Value { get; set; }
    [NotOnWire] public uint Data { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Stat, 5u);
        writer.Write(Type, 2u);

        switch (Type)
        {
            case StatType.Integer:
                writer.Write((uint)Value);
                break;
            case StatType.Float:
                writer.Write(Value);
                break;
            case StatType.Data:
                writer.Write((uint)Value);
                writer.Write(Data);
                break;
        }
    }
}
