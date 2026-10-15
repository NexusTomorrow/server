using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerDialogStart)]
[WireMessage(GameMessageOpcode.ServerDialogStart, Direction.ServerToClient)]
public sealed partial class ServerDialogStart : IWritable
{
    [UInt(32)] public uint DialogUnitId { get; set; }
    [UInt(1)] public bool Unused { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(DialogUnitId);
        writer.Write(Unused);
    }
}
