using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[Message(GameMessageOpcode.ServerCreddRedeemResult)]
public class ServerCreddRedeemResult : IWritable
{
    public CharacterModifyResult Result { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Result, 6u);
    }
}
