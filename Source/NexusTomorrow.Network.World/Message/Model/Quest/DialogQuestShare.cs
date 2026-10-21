using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Quest;

[WireMessage(GameMessageOpcode.Server0363, Direction.ServerToClient)]
public sealed partial class DialogQuestShare
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(15)] public uint Unknown1 { get; set; }
}
