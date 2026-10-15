using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Achievement;

// Client connects to Steam and gets achievements for the game to then pass to the game server
[WireMessage(GameMessageOpcode.ClientSteamAchievements, Direction.ClientToServer)]
public sealed partial class ClientSteamAchievements
{
    [UInt(32)] public uint SteamGameId { get; private set; } // the Steam game ID for Wildstar is 376570
    [Blob(32)] public byte[] AchievementData { get; private set; } = []; // ASCII text
}
