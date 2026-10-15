using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[Message(GameMessageOpcode.ServerCreddRedeemResult)]
[WireMessage(GameMessageOpcode.ServerCreddRedeemResult, Direction.ServerToClient)]
public sealed partial class ServerCreddRedeemResult : IWritable
{
    [UInt(6)] public CharacterModifyResult Result { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Result, 6u);
    }
}
