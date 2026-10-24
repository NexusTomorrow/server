using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Entity.Command;
using NexusTomorrow.Network.World.Message.Model.Chat;
using NexusTomorrow.Network.World.Message.Model.Entity;
using NexusTomorrow.Network.World.Message.Model.Movement;
using NexusTomorrow.Network.World.Message.Model.PlayerPath;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Network.World.Message.Model.WorldEntry;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Harness;

// A client on the world connection, past the greeting and at character select. Every wait is bounded by the connection's receive
// bound, so a server that stays silent fails the scenario.
public sealed class WorldSession : IAsyncDisposable
{
    // The tag the client sends every in-world message under.
    public const byte WorldTag = 0x03;

    // The tag the client sends its realm requests under, chat among them.
    public const byte RealmTag = 0x0B;

    // The client's built-in Say channel.
    private const ushort SayChannelType = 4;

    private const byte TimeCommand = 0;
    private const byte PositionCommand = 2;
    private const byte FacingCommand = 14;

    // Two update intervals of 125 ms: each batch of ClientMovement opens with the client's clock plus both (its remarks).
    private const uint BatchLeadMs = 250;

    private readonly SocketPipe _pipe;

    internal WorldSession(SocketPipe pipe, GameTestClient connection, CharacterList firstList)
    {
        _pipe = pipe;
        Connection = connection;
        FirstList = firstList;
    }

    // The list the server sent when the session reached character select.
    public CharacterList FirstList { get; }

    public GameTestClient Connection { get; }

    public async Task<CharacterList> RequestListAsync()
    {
        await Connection.SendAsync(new CharacterListRequest());
        return await Connection.ExpectAsync<CharacterList>();
    }

    // The client always wraps a create in the nested envelope.
    public async Task<CharacterCreateResult> CreateAsync(string name, uint creationId = 1, byte path = 0)
    {
        await Connection.SendNestedAsync(new CharacterCreate { CreationId = creationId, Name = name, Path = path });
        return await Connection.ExpectAsync<CharacterCreateResult>();
    }

    public async Task<CharacterDeleteResult> DeleteAsync(ulong characterId)
    {
        await Connection.SendAsync(new CharacterDelete { CharacterId = characterId });
        return await Connection.ExpectAsync<CharacterDeleteResult>();
    }

    public async Task<CharacterSelectFailed> SelectAsync(ulong characterId)
    {
        await Connection.SendAsync(new CharacterSelect { CharacterId = characterId });
        return await Connection.ExpectAsync<CharacterSelectFailed>();
    }

    // As the client does: select, take the eight entry messages in order, answer the control grant, report the first takeover's zone
    // change and the world loaded, and wait for the end of the entry.
    public async Task<EnteredWorld> EnterWorldAsync(ulong characterId, ushort zoneId = 7)
    {
        await Connection.SendAsync(new CharacterSelect { CharacterId = characterId });
        var entered = new EnteredWorld(
            await Connection.ExpectAsync<EnterWorld>(),
            await Connection.ExpectAsync<UnitCreate>(),
            await Connection.ExpectAsync<PlayerUnit>(),
            await Connection.ExpectAsync<UnitPathTypeChanged>(),
            await Connection.ExpectAsync<WorldSettings>(),
            await Connection.ExpectAsync<MovementControl>(),
            await Connection.ExpectAsync<PlayerState>(),
            await Connection.ExpectAsync<PathState>());

        await Connection.SendTaggedAsync(WorldTag, ClientMessage.With(new MovementControlReceipt(), nameof(MovementControlReceipt.Ticket), entered.Control.Ticket));
        await Connection.SendTaggedAsync(WorldTag, new ZoneChange());
        await Connection.SendTaggedAsync(WorldTag, ClientMessage.With(new WorldReady(), nameof(WorldReady.ZoneId), zoneId));
        await Connection.ExpectAsync<WorldEntryComplete>();
        return entered;
    }

    // One ClientMovement as the client sends it: a time, a position and a facing of (yaw, 0, 0).
    public Task MoveAsync(Vector3 position, float yaw, uint clock)
    {
        var movement = new ClientMovement { ClientTime = clock };
        movement.Commands.Add(new Server0262Part5 { Unknown0 = TimeCommand, Unknown1 = new SetTimeCommand { Time = clock + BatchLeadMs } });
        movement.Commands.Add(new Server0262Part5 { Unknown0 = PositionCommand, Unknown1 = new SetPositionCommand { Position = position, Blend = true } });
        movement.Commands.Add(new Server0262Part5 { Unknown0 = FacingCommand, Unknown1 = new SetRotationCommand { Rotation = new Vector3(yaw, 0, 0), Blend = true } });
        return Connection.SendTaggedAsync(WorldTag, movement);
    }

    // One line on the Say channel, as the chat input sends it, under the client's id for the line.
    public Task SayAsync(string text, ushort chatId)
    {
        ChatSend say = ClientMessage.With(ClientMessage.With(new ChatSend(), nameof(ChatSend.Text), text), nameof(ChatSend.ChatId), chatId);
        say.Channel.Type = SayChannelType;
        return Connection.SendTaggedAsync(RealmTag, say);
    }

    // As the client does at the Escape menu's Log Out: ask, take the countdown, confirm, and take the logout to character select.
    public async Task<(LogoutCountdown Countdown, WorldLogout Logout)> LogOutAsync()
    {
        await Connection.SendTaggedAsync(WorldTag, ClientMessage.With(new LogoutRequest(), nameof(LogoutRequest.Begin), true));
        LogoutCountdown countdown = await Connection.ExpectAsync<LogoutCountdown>();
        await Connection.SendTaggedAsync(WorldTag, new LogoutConfirm());
        return (countdown, await Connection.ExpectAsync<WorldLogout>());
    }

    public Task<T> ExpectAsync<T>()
        where T : class, IWireMessage => Connection.ExpectAsync<T>();

    public Task<bool> IsClosedAsync(TimeSpan timeout) => Connection.IsClosedAsync(timeout);

    public ValueTask DisposeAsync() => _pipe.DisposeAsync();
}
