using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities
{
    [Message(GameMessageOpcode.ServerAbilityPoints)]
    public class ServerAbilityPoints : IWritable
    {
        public uint AbilityPoints { get; set; }
        public uint TotalAbilityPoints { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(AbilityPoints);
            writer.Write(TotalAbilityPoints);
        }
    }
}
