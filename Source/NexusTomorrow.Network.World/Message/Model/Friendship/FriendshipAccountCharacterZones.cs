using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets the zones that characters on account friends' accounts are in.
/// </summary>
[WireMessage(GameMessageOpcode.Server03A4, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipAccountCharacterZones
{
    [WireStruct]
    public sealed partial class Server03A4Part0
    {
        /// <summary>
        /// The account friend, by <see cref="FriendshipAccountList.FriendshipAccount.Id"/>.
        /// </summary>
        [UInt(64)] public ulong Id { get; set; }

        /// <summary>
        /// The character, by <see cref="FriendshipAccountCharacter.CharacterId"/>.
        /// </summary>
        public RealmScopedId CharacterId { get; set; } = new();

        /// <summary>
        /// The zone the character is in, a WorldZone table id.
        /// </summary>
        [UInt(15)] public uint WorldZoneId { get; set; }
    }

    /// <summary>
    /// The characters and their zones.
    /// </summary>
    [Array(CountBits = 16)] public List<Server03A4Part0> Zones { get; set; } = [];
}
