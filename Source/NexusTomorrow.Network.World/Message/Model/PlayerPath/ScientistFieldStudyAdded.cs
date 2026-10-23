using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server08B5, Direction.ServerToClient)]
public sealed partial class ScientistFieldStudyAdded
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(14)] public uint Unknown1 { get; set; }
}
