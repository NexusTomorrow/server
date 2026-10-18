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
/// One client opcode the gateway admits: its owner, its priority, its rate limit per session and the action past it.
/// </summary>
public sealed record OpcodeRow(ushort Opcode, MessageOwner Owner, MessagePriority Priority, int PerSecond, OverLimitAction OverLimit);

/// <summary>
/// The rows of every client message the gateway admits. A message without a row is dropped and counted.
/// </summary>
public static class OpcodeTable
{
    /// <summary>
    /// The pregame rows: login, world greeting and character select, each a message the client sends untagged. A message the client
    /// sends under a tag arrives as <c>0x038C</c>, which has no row here, so it is dropped.
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
}
