using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerResurrectionState)]
[WireMessage(GameMessageOpcode.ServerResurrectionState, Direction.ServerToClient)]
public sealed partial class ServerResurrectionState : IWritable
{
    [UInt(1)] public bool Forbidden { get; set; }
    [UInt(32)] public ResurrectionType RezType { get; set; }
    [Pad(32)] public Pad RezTypeHigh { get; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Forbidden);
        writer.Write(RezType);
    }
}
