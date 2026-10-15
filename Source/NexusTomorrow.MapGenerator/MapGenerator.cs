using System;
using System.IO;
using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.DependencyInjection;
using NexusTomorrow.MapGenerator.GameTable;
using NLog;

namespace NexusTomorrow.MapGenerator
{
    internal static class MapGenerator
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();
        private static ParserResult<Parameters> parserResult;
        private static IServiceProvider provider;

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

            provider = services.BuildServiceProvider();

            Console.Title = Title;

            parserResult = Parser.Default.ParseArguments<Parameters>(args);
            parserResult.WithParsed(ParameterOk);

            log.Info("Finished!");
        }

        private static void ParameterOk(Parameters parameters)
        {
            if (!Directory.Exists(parameters.PatchPath))
                throw new DirectoryNotFoundException();

            if (!parameters.Extract && !parameters.Generate)
            {
                log.Warn("Please specify the Extract or Generate parameter");
                log.Info(GetHelp());
                return;
            }

            if ((parameters.Extract || parameters.Generate) && !string.IsNullOrEmpty(parameters.OutputDir))
            {
                if (!Directory.Exists(parameters.OutputDir))
                    throw new DirectoryNotFoundException(parameters.OutputDir);
            }

            provider.GetRequiredService<ArchiveManager>().Initialise(parameters.PatchPath);
            provider.GetRequiredService<GameTableManager>().Initialise();

            if (parameters.Extract)
                provider.GetRequiredService<ExtractionManager>().Initialise(parameters.OutputDir);
            if (parameters.Generate)
            {
                GenerationManager generationManager = provider.GetRequiredService<GenerationManager>();
                generationManager.Initialise(parameters.OutputDir);

                var start = DateTime.UtcNow;
                if (parameters.WorldId.HasValue)
                    generationManager.GenerateWorld(parameters.WorldId.Value, parameters.GridX, parameters.GridY);
                else
                    generationManager.GenerateWorlds(true);

                TimeSpan span = DateTime.UtcNow - start;
                log.Info($"Generated base maps in {span.TotalSeconds}s.");
            }
        }

        private static string GetHelp()
        {
            return HelpText.AutoBuild(parserResult, h => h, e => e);
        }
    }
}
