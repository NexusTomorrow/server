using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

// Triggers SettlerInfrastructureComplete lua event
[Message(GameMessageOpcode.ServerPathSettlerInfrastructureComplete)]
[WireMessage(GameMessageOpcode.ServerPathSettlerInfrastructureComplete, Direction.ServerToClient)]
public sealed partial class ServerPathSettlerInfrastructureComplete : IWritable
{
    [UInt(32)] public uint Unused { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Unused);
    }
}
