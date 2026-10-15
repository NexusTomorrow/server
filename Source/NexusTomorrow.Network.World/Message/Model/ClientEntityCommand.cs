using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientEntityCommand)]
[WireMessage(GameMessageOpcode.ClientEntityCommand, Direction.ClientToServer)]
public sealed partial class ClientEntityCommand : IReadable
{
    [UInt(32)] public uint Time { get; set; }
    [Array(CountBits = 32)] public List<NetworkEntityCommand> Commands { get; } = [];

    public void Read(GamePacketReader reader)
    {
        Time = reader.ReadUInt();

        uint commandCount = reader.ReadUInt();
        for (uint i = 0u; i < commandCount; i++)
        {
            EntityCommand command = reader.ReadEnum<EntityCommand>(5);
            IEntityCommandModel model = EntityCommandFactory.NewEntityCommand(command);
            if (model == null)
                return;

            model.Read(reader);
            Commands.Add(new NetworkEntityCommand
            {
                Command = command,
                Model = model
            });
        }
    }
}
