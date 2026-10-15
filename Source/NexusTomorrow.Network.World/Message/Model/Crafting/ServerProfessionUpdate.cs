using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[Message(GameMessageOpcode.ServerProfessionUpdate)]
[WireMessage(GameMessageOpcode.ServerProfessionUpdate, Direction.ServerToClient)]
public sealed partial class ServerProfessionUpdate : IWritable
{
    public TradeskillInfo Tradeskill { get; set; } = new TradeskillInfo();

    public void Write(GamePacketWriter writer)
    {
        Tradeskill.Write(writer);
    }
}
