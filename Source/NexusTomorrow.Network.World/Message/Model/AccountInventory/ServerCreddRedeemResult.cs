using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.ServerCreddRedeemResult, Direction.ServerToClient)]
public sealed partial class ServerCreddRedeemResult
{
    [UInt(6)] public CharacterModifyResult Result { get; set; }
}
