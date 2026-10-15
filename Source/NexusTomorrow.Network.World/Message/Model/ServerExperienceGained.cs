using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerExperienceGained)]
[WireMessage(GameMessageOpcode.ServerExperienceGained, Direction.ServerToClient)]
public sealed partial class ServerExperienceGained : IWritable
{
    [UInt(32)] public uint TotalXpGained { get; set; }
    [UInt(32)] public uint RestXpAmount { get; set; }
    [UInt(32)] public uint SignatureXpAmount { get; set; }
    [UInt(32)] public ExpReason Reason { get; set; }
    [UInt(32)] public uint UnitTarget { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(TotalXpGained);
        writer.Write(RestXpAmount);
        writer.Write(SignatureXpAmount);
        writer.Write(Reason, 32u);
        writer.Write(UnitTarget);
    }
}
