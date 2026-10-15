using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

// Plays the soundEventId set by dataint0 in GameFormula 1263 which happens to be 0 so no sound is played.
// Possibly related to ServerSpellRemove (0x7FE).
// Included for completeness but not useful unless the GameFormula tbl entry is overwritten.
[WireMessage(GameMessageOpcode.Server_0x178_SoundTrigger, Direction.ServerToClient)]
public sealed partial class Server0178SoundTrigger
{
}
