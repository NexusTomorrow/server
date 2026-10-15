using NexusTomorrow.Game.Static.Info;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

[Message(GameMessageOpcode.ServerDisabledGameplaySystems)]
[WireMessage(GameMessageOpcode.ServerDisabledGameplaySystems, Direction.ServerToClient)]
public sealed partial class ServerDisabledGameplaySystems : IWritable
{
    [UInt(8)] public GameplaySystem SystemType { get; set; }
    [UInt(32)] public uint Unused { get; set; }
    [UInt(1)] public bool Disabled { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(SystemType, 8u);
        writer.Write(Unused);
        writer.Write(Disabled);
    }
}
