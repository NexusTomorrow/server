using System.Collections.Frozen;

namespace NexusTomorrow.Gateway.Catalog;

/// <summary>
/// Who handles a client message once the gateway has decoded it.
/// </summary>
public enum MessageOwner
{
    /// <summary>
    /// The connection itself: envelopes, greetings and keep-alives.
    /// </summary>
    Gateway,

    /// <summary>
    /// The account owner.
    /// </summary>
    Account,

    /// <summary>
    /// The realm owner.
    /// </summary>
    Realm,

    /// <summary>
    /// The map that holds the session's character.
    /// </summary>
    Map,
}

/// <summary>
/// How a map drains a message in the world: high is drained in full every tick, normal within a time budget.
/// </summary>
public enum MessagePriority
{
    /// <summary>
    /// Drained in full every tick, held in check by its rate limit.
    /// </summary>
    High,

    /// <summary>
    /// Drained within a time budget per tick, with a guaranteed floor.
    /// </summary>
    Normal,
}

/// <summary>
/// What happens to a message past its row's rate limit.
/// </summary>
public enum OverLimitAction
{
    /// <summary>
    /// The message is dropped and counted.
    /// </summary>
    Drop,

    /// <summary>
    /// The session is disconnected.
    /// </summary>
    Disconnect,
}

/// <summary>
/// One client opcode the gateway admits: its owner, its priority, its rate limit per session and the action past it, and the tag the
/// client sends it under inside <c>TaggedMessage</c>, or null for one sent untagged. The tag is checked as a value: a message under any
/// other tag, or tagged when its row names none, ends the session. <paramref name="ReadsVerifiedMembers"/> is whether the message is
/// admitted at <c>Purpose</c> because its handler reads only members marked verified.
/// </summary>
public sealed record OpcodeRow(
    ushort Opcode, MessageOwner Owner, MessagePriority Priority, int PerSecond, OverLimitAction OverLimit, byte? Tag = null, bool ReadsVerifiedMembers = false);

/// <summary>
/// The rows of every client message the gateway admits. A message without a row is dropped and counted.
/// </summary>
public static class OpcodeTable
{
    /// <summary>
    /// The pregame rows: login, world greeting and character select, each a message the client sends untagged.
    /// </summary>
    public static FrozenDictionary<ushort, OpcodeRow> Pregame { get; } = new OpcodeRow[]
    {
        new(0x0592, MessageOwner.Account, MessagePriority.Normal, 1, OverLimitAction.Disconnect), // LoginRequest
        new(0x058F, MessageOwner.Account, MessagePriority.Normal, 1, OverLimitAction.Disconnect), // WorldHello
        new(0x07E0, MessageOwner.Realm, MessagePriority.Normal, 2, OverLimitAction.Drop), // CharacterListRequest
        new(0x025B, MessageOwner.Realm, MessagePriority.Normal, 1, OverLimitAction.Drop), // CharacterCreate
        new(0x0352, MessageOwner.Realm, MessagePriority.Normal, 1, OverLimitAction.Drop), // CharacterDelete
        new(0x0786, MessageOwner.Realm, MessagePriority.Normal, 1, OverLimitAction.Drop), // CharacterRename
        new(0x07DD, MessageOwner.Realm, MessagePriority.Normal, 1, OverLimitAction.Drop), // CharacterSelect
        new(0x0241, MessageOwner.Gateway, MessagePriority.Normal, 1, OverLimitAction.Drop), // PregameKeepAlive
    }.ToFrozenDictionary(row => row.Opcode);

    /// <summary>
    /// The tag the client sends every message of <see cref="World"/> under.
    /// </summary>
    public const byte WorldTag = 0x03;

    /// <summary>
    /// The in-world rows: each a message the client sends under <see cref="WorldTag"/>, handled by the map that holds the session's
    /// character.
    /// </summary>
    public static FrozenDictionary<ushort, OpcodeRow> World { get; } = new OpcodeRow[]
    {
        new(0x00F2, MessageOwner.Map, MessagePriority.Normal, 2, OverLimitAction.Drop, WorldTag), // WorldReady
        new(0x0635, MessageOwner.Map, MessagePriority.Normal, 4, OverLimitAction.Drop, WorldTag), // MovementControlReceipt
        new(0x063A, MessageOwner.Map, MessagePriority.Normal, 10, OverLimitAction.Drop, WorldTag), // ZoneChange
        new(0x00BF, MessageOwner.Map, MessagePriority.Normal, 2, OverLimitAction.Drop, WorldTag), // LogoutRequest
        new(0x00C0, MessageOwner.Map, MessagePriority.Normal, 2, OverLimitAction.Drop, WorldTag), // LogoutConfirm

        // ClientMovement: admitted at Purpose since its handler reads only MovementReads.Members, each marked; up to every frame while
        // dashing.
        new(0x0637, MessageOwner.Map, MessagePriority.High, 60, OverLimitAction.Drop, WorldTag, ReadsVerifiedMembers: true),
    }.ToFrozenDictionary(row => row.Opcode);

    /// <summary>
    /// Every row: <see cref="Pregame"/> then <see cref="World"/>. Declared after both, so their initializers have run.
    /// </summary>
    public static FrozenDictionary<ushort, OpcodeRow> All { get; } = Pregame.Concat(World).ToFrozenDictionary();
}
