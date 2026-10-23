using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

[WireMessage(GameMessageOpcode.Client0432, Direction.ClientToServer)]
public sealed partial class GroupMarkUnit
{
    [UInt(32)] public uint Unknown0 { get; set; }

    [UInt(32)] public uint Unknown1 { get; set; }
}
