using NexusTomorrow.Game.Static.AccountInventory;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory
{
    [Message(GameMessageOpcode.ServerAccountPrivilegeRestrictionUpdate)]
    public class ServerAccountPrivilegeRestrictionUpdate : IWritable
    {
        public AccountPrivilegeRestriction Restriction { get; set; }
        public float PenaltyDays { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Restriction, 3u);
            writer.Write(PenaltyDays);
        }
    }
}
