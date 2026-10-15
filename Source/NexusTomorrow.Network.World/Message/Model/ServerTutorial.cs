using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerTutorial)]
[WireMessage(GameMessageOpcode.ServerTutorial, Direction.ServerToClient)]
public sealed partial class ServerTutorial : IWritable
{
    [UInt(32)] public uint TutorialId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(TutorialId);
    }
}
