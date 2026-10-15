using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

// Triggers SettlerInfrastructureStarted lua event
[Message(GameMessageOpcode.ServerPathSettlerInfrastructureStarted)]
[WireMessage(GameMessageOpcode.ServerPathSettlerInfrastructureStarted, Direction.ServerToClient)]
public sealed partial class ServerPathSettlerInfrastructureStarted : IWritable
{
    [UInt(32)] public uint Unused { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Unused);
    }
}
