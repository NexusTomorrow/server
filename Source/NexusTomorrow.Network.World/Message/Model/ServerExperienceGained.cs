using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerExperienceGained, Direction.ServerToClient)]
public sealed partial class ServerExperienceGained
{
    [UInt(32)] public uint TotalXpGained { get; set; }
    [UInt(32)] public uint RestXpAmount { get; set; }
    [UInt(32)] public uint SignatureXpAmount { get; set; }
    [UInt(32)] public ExpReason Reason { get; set; }
    [UInt(32)] public uint UnitTarget { get; set; }
}
