using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

[WireMessage(GameMessageOpcode.ServerPlayedResponse, Direction.ServerToClient)]
public sealed partial class ServerPlayedResponse
{
    [UInt(64)] public ulong CreateTime { get; set; } // a Windows file time, as DateTime.ToFileTime gives it
    [UInt(32)] public uint TimePlayedSession { get; set; }
    [UInt(32)] public uint TimePlayedTotal { get; set; }
    [UInt(32)] public uint TimePlayedLevel { get; set; }
}
