using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerEntityCommand)]
[WireMessage(GameMessageOpcode.ServerEntityCommand, Direction.ServerToClient)]
public sealed partial class ServerEntityCommand : IWritable
{
    [UInt(32)] public uint Guid { get; set; }
    [UInt(32)] public uint Time { get; set; }
    [UInt(1)] public bool TimeReset { get; set; }
    [UInt(1)] public bool ServerControlled { get; set; }
    [Array(CountBits = 5)] public List<NetworkEntityCommand> Commands { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Guid);
        writer.Write(Time);
        writer.Write(TimeReset);
        writer.Write(ServerControlled);

        writer.Write((byte)Commands.Count, 5u);
        foreach (INetworkEntityCommand command in Commands)
        {
            writer.Write(command.Command, 5);
            command.Model.Write(writer);
        }
    }
}
