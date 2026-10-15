using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

/// <summary>
/// A stat and its value as <see cref="ServerEntityStatUpdateFloat"/> carries it. No selector is on the wire: the opcode picks the encoding,
/// and <see cref="ServerEntityStatUpdateInteger"/> carries its own integer layout.
/// </summary>
[WireStruct]
public sealed partial class StatValueUpdate : IWritable
{
    [UInt(5)] public Stat Stat { get; set; }
    [NotOnWire] public StatType Type { get; set; }
    [Float32] public float Value { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Stat, 5u);

        switch (Type)
        {
            case StatType.Integer:
                writer.Write((uint)Value);
                break;
            case StatType.Float:
                writer.Write(Value);
                break;
        }
    }
}
