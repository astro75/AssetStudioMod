using AssetStudio;
using AssetStudioCLI.Options;
using System;

namespace AssetStudioCLI
{
    class Program
    {
        public static void Main(string[] args)
        {
            CLIOptions.ParseArgs(args);
            if (CLIOptions.isParsed)
            {
                CLIRun();
            }
            else if (CLIOptions.showHelp)
            {
                CLIOptions.ShowHelp();
            }
            else
            {
                Console.WriteLine();
                CLIOptions.ShowHelp(showUsageOnly: true);
            }
        }

        private static void CLIRun()
        {
            var cliLogger = new CLILogger();
            Logger.Default = cliLogger;
            CLIOptions.ShowCurrentOptions();

            try
            {
                if (Studio.LoadAssets())
                {
                    Studio.ParseAssets();
                    if (CLIOptions.filterBy != FilterBy.None)
                    {
                        Studio.Filter();
                    }
                    if (CLIOptions.o_exportAssetList.Value != ExportListType.None)
                    {
                        Studio.ExportAssetList();
                    }
                    switch (CLIOptions.o_workMode.Value)
                    {
                        case WorkMode.Info:
                            Studio.ShowExportableAssetsInfo();
                            break;
                        case WorkMode.Live2D:
                            Studio.ExportLive2D();
                            break;
                        case WorkMode.SplitObjects:
                            Studio.ExportSplitObjects();
                            break;
                        default:
                            Studio.ExportAssets();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
            finally
            {
                cliLogger.LogToFile(LoggerEvent.Verbose, "---Program ended---");
            }
        }       
    }
}
