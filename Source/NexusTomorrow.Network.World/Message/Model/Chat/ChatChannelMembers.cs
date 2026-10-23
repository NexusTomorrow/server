using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat;

[WireMessage(GameMessageOpcode.Server01C0, Direction.ServerToClient)]
public sealed partial class ChatChannelMembers
{
    public ChatChannelKey Unknown0 { get; set; } = new();
    [UInt(32), Count] public uint Unknown1 { get; set; }
    [Array(CountFrom = nameof(Unknown1)), WideString] public List<string> Unknown2 { get; set; } = [];
    [Array(CountFrom = nameof(Unknown1)), UInt(32)] public List<uint> Unknown3 { get; set; } = [];
    [UInt(1)] public bool Unknown4 { get; set; }
}
