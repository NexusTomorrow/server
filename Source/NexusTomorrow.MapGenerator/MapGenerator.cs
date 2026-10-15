using System;
using System.IO;
using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.DependencyInjection;
using NexusTomorrow.MapGenerator.GameTable;
using NLog;

namespace NexusTomorrow.MapGenerator;

internal static class MapGenerator
{
    private static readonly Logger s_log = LogManager.GetCurrentClassLogger();

#if DEBUG
    private const string Title = "NexusTomorrow: Map Generator (DEBUG)";
#else
    private const string Title = "NexusTomorrow: Map Generator (RELEASE)";
#endif

    private static void Main(string[] args)
    {
        IServiceCollection services = new ServiceCollection();
        services.AddSingleton<ArchiveManager>();
        services.AddSingleton<GameTableManager>();
        services.AddSingleton<ExtractionManager>();
        services.AddSingleton<GenerationManager>();

        IServiceProvider provider = services.BuildServiceProvider();

        Console.Title = Title;

        ParserResult<Parameters> parserResult = Parser.Default.ParseArguments<Parameters>(args);
        parserResult.WithParsed(parameters => ParameterOk(parameters, provider, parserResult));

        s_log.Info("Finished!");
    }

    private static void ParameterOk(Parameters parameters, IServiceProvider provider, ParserResult<Parameters> parserResult)
    {
        if (!Directory.Exists(parameters.PatchPath))
            throw new DirectoryNotFoundException();

        if (!parameters.Extract && !parameters.Generate)
        {
            s_log.Warn("Please specify the Extract or Generate parameter");
            s_log.Info(GetHelp(parserResult));
            return;
        }

        if ((parameters.Extract || parameters.Generate) && !string.IsNullOrEmpty(parameters.OutputDir))
        {
            if (!Directory.Exists(parameters.OutputDir))
                throw new DirectoryNotFoundException(parameters.OutputDir);
        }

        provider.GetRequiredService<ArchiveManager>().Initialize(parameters.PatchPath);
        provider.GetRequiredService<GameTableManager>().Initialize();

        if (parameters.Extract)
            provider.GetRequiredService<ExtractionManager>().Initialize(parameters.OutputDir);
        if (parameters.Generate)
        {
            GenerationManager generationManager = provider.GetRequiredService<GenerationManager>();
            generationManager.Initialize(parameters.OutputDir);

            DateTime start = DateTime.UtcNow;
            if (parameters.WorldId.HasValue)
                generationManager.GenerateWorld(parameters.WorldId.Value, parameters.GridX, parameters.GridY);
            else
                generationManager.GenerateWorlds(true);

            TimeSpan span = DateTime.UtcNow - start;
            s_log.Info($"Generated base maps in {span.TotalSeconds}s.");
        }
    }

    private static string GetHelp(ParserResult<Parameters> parserResult)
    {
        return HelpText.AutoBuild(parserResult, h => h, e => e);
    }
}
