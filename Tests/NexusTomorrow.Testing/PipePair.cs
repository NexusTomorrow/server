using System.IO.Pipelines;

namespace NexusTomorrow.Testing;

// Two pipes wired back to back: what the client writes the server reads, and the reverse.
public sealed class PipePair
{
    public PipePair()
    {
        var toServer = new Pipe();
        var toClient = new Pipe();
        Server = new Duplex(toServer.Reader, toClient.Writer);
        Client = new Duplex(toClient.Reader, toServer.Writer);
    }

    public IDuplexPipe Server { get; }

    public IDuplexPipe Client { get; }

    private sealed record Duplex(PipeReader Input, PipeWriter Output) : IDuplexPipe;
}
