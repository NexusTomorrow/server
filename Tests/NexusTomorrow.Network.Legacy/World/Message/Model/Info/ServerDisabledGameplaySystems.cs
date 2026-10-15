using NexusTomorrow.Game.Static.Info;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Info;

[Message(GameMessageOpcode.ServerDisabledGameplaySystems)]
public class ServerDisabledGameplaySystems : IWritable
{
    public GameplaySystem SystemType { get; set; }
    public uint Unused { get; set; }
    public bool Disabled { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(SystemType, 8u);
        writer.Write(Unused);
        writer.Write(Disabled);
    }
}
