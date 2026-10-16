using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Leaderboard;

[WireStruct]
public sealed partial class Server0579Part1
{
    [WideString] public string Unknown0 { get; set; } = "";
    [UInt(5)] public byte Unknown1 { get; set; }
}
