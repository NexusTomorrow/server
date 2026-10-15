using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

[Message(GameMessageOpcode.ServerCharacterDisabled)]
[WireMessage(GameMessageOpcode.ServerCharacterDisabled, Direction.ServerToClient)]
public sealed partial class ServerCharacterDisabled : IWritable
{
    [UInt(64)] public ulong CharacterId { get; set; }
    [UInt(1)] public bool Disabled { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CharacterId);
        writer.Write(Disabled);
    }
}
