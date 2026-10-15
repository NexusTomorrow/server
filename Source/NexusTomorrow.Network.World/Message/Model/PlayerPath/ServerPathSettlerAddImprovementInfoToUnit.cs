using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[Message(GameMessageOpcode.ServerPathSettlerAddImprovementInfoToUnit)]
[WireMessage(GameMessageOpcode.ServerPathSettlerAddImprovementInfoToUnit, Direction.ServerToClient)]
public sealed partial class ServerPathSettlerAddImprovementInfoToUnit : IWritable
{
    [WireStruct]
    public sealed partial class ImprovementInfo : IWritable
    {
        [WideString] public string Name { get; set; }
        [UInt(32)] public uint Tier { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.WriteStringWide(Name);
            writer.Write(Tier);
        }
    }

    [UInt(32)] public uint UnitId { get; set; }
    [UInt(14)] public uint PathSettlerImprovementGroupId { get; set; }
    [UInt(32)] public uint RemainingTime { get; set; }
    [Array(CountBits = 32), WideString] public List<string> Owners { get; set; } = [];
    [Array(CountBits = 32)] public List<ImprovementInfo> Improvements { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(PathSettlerImprovementGroupId, 14);
        writer.Write(RemainingTime);

        writer.Write(Owners.Count);
        Owners.ForEach(owner => writer.WriteStringWide(owner));

        writer.Write(Improvements.Count);
        Improvements.ForEach(improvement => improvement.Write(writer));
    }
}
