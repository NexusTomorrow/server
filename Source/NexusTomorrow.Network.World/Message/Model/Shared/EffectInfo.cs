using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Spell;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class EffectInfo : IWritable
{
    /// <summary>
    /// The effect's result, present when <see cref="InfoType"/> is 1; only cases 0 and 1 have a known layout.
    /// </summary>
    public interface IPayload : IWireStruct
    {
    }

    [WireStruct]
    [UnionCase(0)]
    public sealed partial class NoResult : IPayload
    {
        [Pad(1)] public Pad Unknown { get; }
    }

    [WireStruct]
    [UnionCase(1)]
    public sealed partial class Result : IPayload
    {
        public SpellEffectResult EffectResultData { get; set; } = new();
    }

    [UInt(19)] public uint Spell4EffectId { get; set; }
    [UInt(32)] public uint EffectUniqueId { get; set; }
    [UInt(32)] public uint DelayTime { get; set; }
    [UInt(32)] public int TimeRemaining { get; set; }
    [UInt(2)] public byte InfoType { get; set; }
    [Union(nameof(InfoType))] public IPayload Payload { get; set; } = new NoResult();

    [NotOnWire] public SpellEffectResult EffectResultData { get; set; } = new();

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Spell4EffectId, 19u);
        writer.Write(EffectUniqueId);
        writer.Write(DelayTime);
        writer.Write(TimeRemaining);
        writer.Write(InfoType, 2u);

        if (InfoType == 1)
            EffectResultData.Write(writer);
        else
            writer.Write(0u, 1u);
    }

}
