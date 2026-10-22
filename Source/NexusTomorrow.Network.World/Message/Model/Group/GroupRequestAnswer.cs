using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

[WireMessage(GameMessageOpcode.Client0453, Direction.ClientToServer)]
public sealed partial class GroupRequestAnswer
{
    [UInt(64)] public ulong Unknown0 { get; set; }

    [UInt(1)] public bool Unknown1 { get; set; }

    [WideString] public string Unknown2 { get; set; } = "";

    [UInt(1)] public bool Unknown3 { get; set; }
}
