using NexusTomorrow.Game.Static.Pvp;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp;

// Send to all observers in the nearby area. Client handles the correctness of the UI messages.
[Message(GameMessageOpcode.ServerDuelResult)]
[WireMessage(GameMessageOpcode.ServerDuelResult, Direction.ServerToClient)]
public sealed partial class ServerDuelResult : IWritable
{
    [UInt(32)] public uint WinnerUnitId { get; set; }
    [UInt(32)] public uint LoserUnitId { get; set; }
    [UInt(3)] public DuelFinishReason Reason { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(WinnerUnitId);
        writer.Write(LoserUnitId);
        writer.Write(Reason, 3u);
    }
}
