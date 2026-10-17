using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Testing;

namespace NexusTomorrow.Harness;

// A client on the world connection, past the greeting and at character select. Every wait is bounded by the connection's receive
// bound, so a server that stays silent fails the scenario.
public sealed class WorldSession : IAsyncDisposable
{
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

    public Task<T> ExpectAsync<T>()
        where T : class, IWireMessage => Connection.ExpectAsync<T>();

    public Task<bool> IsClosedAsync(TimeSpan timeout) => Connection.IsClosedAsync(timeout);

    public ValueTask DisposeAsync() => _pipe.DisposeAsync();
}
