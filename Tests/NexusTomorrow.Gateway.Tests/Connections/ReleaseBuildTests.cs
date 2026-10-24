#if !DEBUG
using System.Reflection;
using NexusTomorrow.Gateway.Connections;

namespace NexusTomorrow.Gateway.Tests.Connections;

// Compiled into Release builds only, where the workflow runs it: no send in a release skips the send gate.
public sealed class ReleaseBuildTests
{
    [Fact]
    public void AReleaseGatewayHasNoDebugSessionSurface()
    {
        const BindingFlags Every = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        Assembly gateway = typeof(ClientConnection).Assembly;

        Assert.Null(gateway.GetType("NexusTomorrow.Gateway.Connections.IDebugSession"));
        Assert.Null(gateway.GetType("NexusTomorrow.Gateway.Connections.ISessionTap"));
        Assert.DoesNotContain(typeof(ClientConnection).GetMethods(Every), method => method.Name is "SendUnchecked" or "SendRaw");
        Assert.DoesNotContain(typeof(Catalog.InboundDecoder).GetMethods(Every), method => method.Name == "TryOpenClear");
    }
}
#endif
