using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities
{
    [Message(GameMessageOpcode.ServerAmpPowerUpdate)]
    public class ServerAmpPowerUpdate : IWritable
    {
        public ushort BonusPower { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(BonusPower);
        }
    }
}
