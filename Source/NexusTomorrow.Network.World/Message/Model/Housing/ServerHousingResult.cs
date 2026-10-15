using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ServerHousingResult)]
[WireMessage(GameMessageOpcode.ServerHousingResult, Direction.ServerToClient)]
public sealed partial class ServerHousingResult : IWritable
{
    public Identity ResidenceIdentity { get; set; } // not used by client
    [WideString] public string PlayerName { get; set; }
    [UInt(7)] public HousingResult Result { get; set; }

    public void Write(GamePacketWriter writer)
    {
        ResidenceIdentity.Write(writer);
        writer.WriteStringWide(PlayerName);
        writer.Write(Result, 7u);
    }
}
