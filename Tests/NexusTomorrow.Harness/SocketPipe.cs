using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;

namespace NexusTomorrow.Harness;

// A TCP connection to the harness server as a duplex pipe.
public sealed class SocketPipe : IDuplexPipe, IAsyncDisposable
{
    private static readonly TimeSpan s_connectBound = TimeSpan.FromSeconds(10);

    private readonly NetworkStream _stream;

    private SocketPipe(NetworkStream stream)
    {
        _stream = stream;
        Input = PipeReader.Create(stream);
        Output = PipeWriter.Create(stream);
    }

    public PipeReader Input { get; }

    public PipeWriter Output { get; }

    // Connect within a bound, so a server that does not answer fails the scenario instead of hanging it.
    public static async Task<SocketPipe> ConnectAsync(IPAddress address, int port)
    {
        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) { NoDelay = true };
        try
        {
            using var bound = new CancellationTokenSource(s_connectBound);
            await socket.ConnectAsync(address, port, bound.Token);
            return new SocketPipe(new NetworkStream(socket, ownsSocket: true));
        }
        catch
        {
            socket.Dispose();
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await Output.CompleteAsync();
        await Input.CompleteAsync();
        await _stream.DisposeAsync();
    }
}
