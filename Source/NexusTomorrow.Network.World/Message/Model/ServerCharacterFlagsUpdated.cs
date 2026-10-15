using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerCharacterFlagsUpdated)]
[WireMessage(GameMessageOpcode.ServerCharacterFlagsUpdated, Direction.ServerToClient)]
public sealed partial class ServerCharacterFlagsUpdated : IWritable
{
    [UInt(32)] public CharacterFlag Flags { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Flags, 32u);
    }
}
