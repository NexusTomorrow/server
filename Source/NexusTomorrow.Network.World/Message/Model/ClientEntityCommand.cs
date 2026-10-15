using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ClientEntityCommand)]
    public class ClientEntityCommand : IReadable
    {
        public uint Time { get; set; }
        public List<INetworkEntityCommand> Commands { get; } = new();

        public void Read(GamePacketReader reader)
        {
            Time = reader.ReadUInt();

            uint commandCount = reader.ReadUInt();
            for (uint i = 0u; i < commandCount; i++)
            {
                EntityCommand command     = reader.ReadEnum<EntityCommand>(5);
                IEntityCommandModel model = EntityCommandFactory.NewEntityCommand(command);
                if (model == null)
                    return;

                model.Read(reader);
                Commands.Add(new NetworkEntityCommand
                {
                    Command = command,
                    Model   = model
                });
            }
        }
    }
}
