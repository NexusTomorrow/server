using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Fortune;

// Not sure the level of use of this message
// Triggers ClickEmpty sound if fortune game is not started or when value is 3
[Message(GameMessageOpcode.ServerFortuneReset)]
[WireMessage(GameMessageOpcode.ServerFortuneReset, Direction.ServerToClient)]
public sealed partial class ServerFortuneReset : IWritable
{
    [UInt(3)] public uint Unknown { get; set; } // 3 = click empty sound, only seen value 2 in sniffs

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Unknown, 3u);
    }
}
