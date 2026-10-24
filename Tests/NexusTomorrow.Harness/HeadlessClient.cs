using System.Buffers.Binary;
using System.Globalization;
using System.Net;
using NexusTomorrow.Network;
using NexusTomorrow.Network.Auth.Message.Model;
using NexusTomorrow.Network.Message.Model;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Testing;
using NexusTomorrow.Transport;

namespace NexusTomorrow.Harness;

// A client that speaks the real protocol over sockets: the STS login, then the login connection, then the world connection. The STS
// and login connections stay open for the client's lifetime, as the real client keeps them.
public sealed class HeadlessClient : IAsyncDisposable
{
    private const uint Build = 16042;

    private readonly HarnessServer _server;
    private readonly List<SocketPipe> _pipes = [];

    internal HeadlessClient(HarnessServer server)
    {
        _server = server;
    }

    public async Task<(Guid? Token, StsReply? Error)> StsLoginAsync(string email, string password)
    {
        SocketPipe pipe = await ConnectAsync(IPAddress.Loopback, _server.StsPort);
        return await new StsTestClient(pipe).LoginAsync(email, password);
    }

    public async Task<(RealmHandoff? Handoff, LoginDenied? Denied)> LoginAsync(Guid token, string email)
    {
        SocketPipe pipe = await ConnectAsync(IPAddress.Loopback, _server.LoginPort);
        var game = new GameTestClient(pipe);
        await game.ExpectAsync<ServerHello>();

        var request = new LoginRequest { Build = Build, AccountName = email };
        Assign(request.GameToken, NetworkGuid.FromGuid(token));
        await game.SendAsync(request);

        Received answer = await game.ReceiveAsync();
        switch (answer.Message)
        {
            case LoginDenied denied:
                return (null, denied);
            case LoginAccepted:
                return (await game.ExpectAsync<RealmHandoff>(), null);
            default:
                throw new InvalidOperationException($"The login connection answered 0x{answer.Opcode:X4}, neither an acceptance nor a denial.");
        }
    }

    public async Task<WorldSession> EnterCharacterSelectAsync(RealmHandoff handoff, string email)
    {
        // The handoff's address is the integer whose high byte is the first octet.
        byte[] octets = new byte[4];
        BinaryPrimitives.WriteUInt32BigEndian(octets, handoff.WorldAddress);
        SocketPipe pipe = await ConnectAsync(new IPAddress(octets), handoff.WorldPort);
        try
        {
            var game = new GameTestClient(pipe);
            await game.ExpectAsync<ServerHello>();

            var hello = new WorldHello { AccountId = handoff.Echo, AccountName = email };
            Assign(hello.SessionKey, handoff.SessionKey);
            await game.SendAsync(hello);

            // The server reads everything after the greeting under the session key.
            byte[] key = new byte[16];
            handoff.SessionKey.CopyTo(key);
            game.Cipher = new PacketCipher(PacketCipher.SessionKey(key));

            await game.SendAsync(new CharacterListRequest());
            return new WorldSession(pipe, game, await game.ExpectAsync<CharacterList>());
        }
        catch
        {
            await pipe.DisposeAsync();
            throw;
        }
    }

    // The three steps; a step that fails throws with what the server answered. The password appears in no message.
    public async Task<WorldSession> ScriptedLoginAsync(string email, string password)
    {
        (Guid? token, StsReply? error) = await StsLoginAsync(email, password);
        if (token is null)
            throw new InvalidOperationException($"The STS refused the login: status {error?.Status.ToString(CultureInfo.InvariantCulture)}.");

        (RealmHandoff? handoff, LoginDenied? denied) = await LoginAsync(token.Value, email);
        if (handoff is null)
            throw new InvalidOperationException($"The login server denied the login: result {denied?.Result.ToString(CultureInfo.InvariantCulture)}.");

        return await EnterCharacterSelectAsync(handoff, email);
    }

    public async ValueTask DisposeAsync()
    {
        for (int i = _pipes.Count - 1; i >= 0; i--)
            await _pipes[i].DisposeAsync();
        _pipes.Clear();
    }

    private async Task<SocketPipe> ConnectAsync(IPAddress address, int port)
    {
        SocketPipe pipe = await SocketPipe.ConnectAsync(address, port);
        _pipes.Add(pipe);
        return pipe;
    }

    // NetworkGuid's fields are get-only on messages, so a copy goes field by field.
    private static void Assign(NetworkGuid target, NetworkGuid source)
    {
        target.Data1 = source.Data1;
        target.Data2 = source.Data2;
        target.Data3 = source.Data3;
        target.Data4 = source.Data4;
    }
}
