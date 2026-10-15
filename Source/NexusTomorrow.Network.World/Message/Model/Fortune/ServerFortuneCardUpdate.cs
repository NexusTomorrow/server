using NexusTomorrow.Game.Static.Fortune;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Fortune;

[WireMessage(GameMessageOpcode.ServerFortuneCardUpdate, Direction.ServerToClient)]
public sealed partial class ServerFortuneCardUpdate
{
    [UInt(1)] public bool Unknown { get; set; } = true; // always needs to be true or the client ignores the rest of the message
    [UInt(3)] public FortuneOperation Operation { get; set; }
    [FixedArray(3), UInt(1)] public bool[] CardFlipped { get; set; } = new bool[3];
}
