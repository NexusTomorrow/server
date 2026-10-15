using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[Message(GameMessageOpcode.ServerSpecChanged)]
[WireMessage(GameMessageOpcode.ServerSpecChanged, Direction.ServerToClient)]
public sealed partial class ServerSpecChanged : IWritable
{
    [UInt(3)] public byte ActionSetIndex { get; set; }
    [UInt(32)] public SpecError SpecError { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ActionSetIndex, 3u);
        writer.Write(SpecError, 32u);
    }
}
