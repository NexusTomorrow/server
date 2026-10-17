using Microsoft.AspNetCore.Builder;
using NexusTomorrow.Server;

if (args is ["migrate", ..])
{
    WebApplicationBuilder builder = WebApplication.CreateSlimBuilder(args[1..]);
    return await ServerHost.MigrateAsync(builder.Configuration, CancellationToken.None);
}

await using WebApplication app = ServerHost.Build(args);
await app.RunAsync();
return 0;
