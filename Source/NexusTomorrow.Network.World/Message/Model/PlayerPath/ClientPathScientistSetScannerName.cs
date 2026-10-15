using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

// While PetType has multiple values, client only has code to use this for scanbots
[WireMessage(GameMessageOpcode.ClientPathScientistSetScannerName, Direction.ClientToServer)]
public sealed partial class ClientPathScientistSetScannerName
{
    [UInt(2)] public PetType PetType { get; private set; }
    [UInt(32)] public uint PathScientistScanBotProfileId { get; private set; }
    [WideString] public string Name { get; private set; }
}
