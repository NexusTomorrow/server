using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ServerHousingResult, Direction.ServerToClient)]
public sealed partial class ServerHousingResult
{
    public Identity ResidenceIdentity { get; set; } // not used by client
    [WideString] public string PlayerName { get; set; }
    [UInt(7)] public HousingResult Result { get; set; }
}
