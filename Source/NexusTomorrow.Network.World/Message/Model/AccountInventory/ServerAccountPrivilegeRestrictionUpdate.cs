using NexusTomorrow.Game.Static.AccountInventory;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[Message(GameMessageOpcode.ServerAccountPrivilegeRestrictionUpdate)]
[WireMessage(GameMessageOpcode.ServerAccountPrivilegeRestrictionUpdate, Direction.ServerToClient)]
public sealed partial class ServerAccountPrivilegeRestrictionUpdate : IWritable
{
    [UInt(3)] public AccountPrivilegeRestriction Restriction { get; set; }
    [Float32] public float PenaltyDays { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Restriction, 3u);
        writer.Write(PenaltyDays);
    }
}
