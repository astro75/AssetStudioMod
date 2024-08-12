using AssetStudio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AssetStudioCLI.Options
{
    internal enum HelpGroups
    {
        General,
        Convert,
        Logger,
        Live2D,
        FBX,
        Filter,
        Advanced,
    }

    internal enum WorkMode
    {
        Export,
        ExportRaw,
        Dump,
        Info,
        Live2D,
        SplitObjects,
    }

    internal enum AssetGroupOption
    {
        None,
        TypeName,
        ContainerPath,
        ContainerPathFull,
        SourceFileName,
        SceneHierarchy,
    }

    internal enum FilenameFormat
    {
        AssetName,
        AssetName_PathID,
        PathID,
    }

    internal enum ExportListType
    {
        None,
        XML,
    }

    internal enum AudioFormat
    {
        None,
        Wav,
    }

    internal enum FilterBy
    {
        None,
        Name,
        Container,
        PathID,
        NameOrContainer,
        NameAndContainer,
    }

    internal enum CustomCompressionType
    {
        Zstd,
        Lz4,
    }

    internal static class CLIOptions
    {
        public static bool isParsed;
        public static bool showHelp;
        public static string[] cliArgs;
        public static string inputPath;
        public static FilterBy filterBy;
        private static Dictionary<string, string> optionsDict;
        private static Dictionary<string, string> flagsDict;
        private static Dictionary<HelpGroups, Dictionary<string, string>> optionGroups;
        private static List<ClassIDType> exportableAssetTypes;
        private static Dictionary<string, ClassIDType> knownAssetTypesDict;
        //general
        public static Option<WorkMode> o_workMode;
        public static Option<List<ClassIDType>> o_exportAssetTypes;
        public static Option<AssetGroupOption> o_groupAssetsBy;
        public static Option<FilenameFormat> o_filenameFormat;
        public static Option<string> o_outputFolder;
        public static Option<bool> o_displayHelp;
        //logger
        public static Option<LoggerEvent> o_logLevel;
        public static Option<LogOutputMode> o_logOutput;
        //convert
        public static bool convertTexture;
        public static Option<ImageFormat> o_imageFormat;
        public static Option<AudioFormat> o_audioFormat;
        //live2d
        public static Option<CubismLive2DExtractor.Live2DMotionMode> o_l2dMotionMode;
        public static Option<bool> f_l2dForceBezier;
        //fbx
        public static Option<float> o_fbxScaleFactor;
        public static Option<int> o_fbxBoneSize;
        //filter
        public static Option<List<string>> o_filterByName;
        public static Option<List<string>> o_filterByContainer;
        public static Option<List<string>> o_filterByPathID;
        public static Option<List<string>> o_filterByText;
        //advanced
        public static Option<CustomCompressionType> o_customCompressionType;
        public static Option<int> o_maxParallelExportTasks;
        public static Option<ExportListType> o_exportAssetList;
        public static Option<string> o_assemblyPath;
        public static Option<UnityVersion> o_unityVersion;
        public static Option<bool> f_notRestoreExtensionName;
        public static Option<bool> f_avoidLoadingViaTypetree;
        public static Option<bool> f_loadAllAssets;

        static CLIOptions()
        {
            OptionExtensions.OptionGrouping = OptionGrouping;
            InitOptions();
        }

        private static void OptionGrouping(string name, string desc, string example, HelpGroups helpGroup, bool isFlag)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            var optionDesc = desc + example.Color(ColorConsole.BrightBlack);
            var optionDict = new Dictionary<string, string>() { { name, optionDesc } };
            if (optionGroups.TryGetValue(helpGroup, out Dictionary<string, string> groupDict))
            {
                groupDict.Add(name, optionDesc);
            }
            else
            {
                optionGroups.Add(helpGroup, optionDict);
            }

            if (isFlag)
            {
                flagsDict.Add(name, optionDesc);
            }
            else
            {
                optionsDict.Add(name, optionDesc);
            }
        }

        private static void InitOptions()
        {
            isParsed = false;
            showHelp = false;
            cliArgs = null;
            inputPath = "";
            filterBy = FilterBy.None;
            optionsDict = new Dictionary<string, string>();
            flagsDict = new Dictionary<string, string>();
            optionGroups = new Dictionary<HelpGroups, Dictionary<string, string>>();
            exportableAssetTypes = new List<ClassIDType>
            {
                ClassIDType.Texture2D,
                ClassIDType.Texture2DArray,
                ClassIDType.Sprite,
                ClassIDType.TextAsset,
                ClassIDType.MonoBehaviour,
                ClassIDType.Font,
                ClassIDType.Shader,
                ClassIDType.AudioClip,
                ClassIDType.VideoClip,
                ClassIDType.MovieTexture,
                ClassIDType.Mesh,
            };
            knownAssetTypesDict = ((ClassIDType[])Enum.GetValues(typeof(ClassIDType))).ToHashSet().ToDictionary(x => x.ToString().ToLower(), y => y);

            #region Init General Options
            o_workMode = new GroupedOption<WorkMode>
            (
                optionDefaultValue: WorkMode.Export,
                optionName: "-m, --mode <value>",
                optionDescription: "Specify working mode\n" +
                    "<Value: export(default) | exportRaw | dump | info | live2d | splitObjects>\n" +
                    "Export - Exports converted assets\n" +
                    "ExportRaw - Exports raw data\n" +
                    "Dump - Makes asset dumps\n" +
                    "Info - Loads file(s), shows the number of available for export assets and exits\n" +
                    "Live2D - Exports Live2D Cubism models\n" +
                    "SplitObjects - Exports split objects (fbx)\n",
                optionExample: "Example: \"-m info\"\n",
                optionHelpGroup: HelpGroups.General
            );
            o_exportAssetTypes = new GroupedOption<List<ClassIDType>>
            (
                optionDefaultValue: exportableAssetTypes,
                optionName: "-t, --asset-type <value(s)>",
                optionDescription: "Specify asset type(s) to export\n" +
                    "<Value(s): tex2d, tex2dArray, sprite, textAsset, monoBehaviour, font, shader\n" +
                    "movieTexture, audio, video, mesh | all(default)>\n" +
                    "All - export all asset types, which are listed in the values\n" +
                    "*To specify multiple asset types, write them separated by ',' or ';' without spaces\n",
                optionExample: "Examples: \"-t sprite\" or \"-t tex2d,sprite,audio\" or \"-t tex2d;sprite;font\"\n",
                optionHelpGroup: HelpGroups.General
            );
            o_groupAssetsBy = new GroupedOption<AssetGroupOption>
            (
                optionDefaultValue: AssetGroupOption.ContainerPath,
                optionName: "-g, --group-option <value>",
                optionDescription: "Specify the way in which exported assets should be grouped\n" +
                    "<Value: none | type | container(default) | containerFull | filename | sceneHierarchy>\n" +
                    "None - Do not group exported assets\n" +
                    "Type - Group exported assets by type name\n" +
                    "Container - Group exported assets by container path\n" +
                    "ContainerFull - Group exported assets by full container path (e.g. with prefab name)\n" +
                    "SceneHierarchy - Group exported assets by their node path in scene hierarchy\n" +
                    "Filename - Group exported assets by source file name\n",
                optionExample: "Example: \"-g containerFull\"\n",
                optionHelpGroup: HelpGroups.General
            );
            o_filenameFormat = new GroupedOption<FilenameFormat>
            (
                optionDefaultValue: FilenameFormat.AssetName,
                optionName: "-f, --filename-format <value>",
                optionDescription: "Specify the file name format for exported assets\n" +
                                   "<Value: assetName(default) | assetName_pathID | pathID>\n" +
                                   "AssetName - Asset file names will look like \"assetName.extension\"\n" +
                                   "AssetName_pathID - Asset file names will look like \"assetName @pathID.extension\"\n" +
                                   "PathID - Asset file names will look like \"pathID.extension\"\n",
                optionExample: "Example: \"-f assetName_pathID\"\n",
                optionHelpGroup: HelpGroups.General
            );
            o_outputFolder = new GroupedOption<string>
            (
                optionDefaultValue: "ASExport",
                optionName: "-o, --output <path>",
                optionDescription: "Specify path to the output folder\n" +
                    "If path isn't specified, 'ASExport' folder will be created in the program's work folder\n",
                optionExample: "",
                optionHelpGroup: HelpGroups.General
            );
            o_displayHelp = new GroupedOption<bool>
            (
                optionDefaultValue: false,
                optionName: "-h, --help",
                optionDescription: "Display help and exit",
                optionExample: "",
                optionHelpGroup: HelpGroups.General
            );
            #endregion

            #region Init Logger Options
            o_logLevel = new GroupedOption<LoggerEvent>
            (
                optionDefaultValue: LoggerEvent.Info,
                optionName: "--log-level <value>",
                optionDescription: "Specify the log level\n" +
                    "<Value: verbose | debug | info(default) | warning | error>\n",
                optionExample: "Example: \"--log-level warning\"\n",
                optionHelpGroup: HelpGroups.Logger
            );
            o_logOutput = new GroupedOption<LogOutputMode> 
            (
                optionDefaultValue: LogOutputMode.Console,
                optionName: "--log-output <value>",
                optionDescription: "Specify the log output\n" +
                    "<Value: console(default) | file | both>\n",
                optionExample: "Example: \"--log-output both\"",
                optionHelpGroup: HelpGroups.Logger
            );
            #endregion

            #region Init Convert Options
            convertTexture = true;
            o_imageFormat = new GroupedOption<ImageFormat>
            (
                optionDefaultValue: ImageFormat.Png,
                optionName: "--image-format <value>",
                optionDescription: "Specify the format for converting image assets\n" +
                    "<Value: none | jpg | png(default) | bmp | tga | webp>\n" +
                    "None - Do not convert images and export them as texture data (.tex)\n",
                optionExample: "Example: \"--image-format jpg\"\n",
                optionHelpGroup: HelpGroups.Convert
            );
            o_audioFormat = new GroupedOption<AudioFormat>
            (
                optionDefaultValue: AudioFormat.Wav,
                optionName: "--audio-format <value>",
                optionDescription: "Specify the format for converting FMOD audio assets\n" +
                    "<Value: none | wav(default)>\n" +
                    "None - Do not convert fmod audios and export them in their own format\n",
                optionExample: "Example: \"--audio-format wav\"",
                optionHelpGroup: HelpGroups.Convert
            );
            #endregion

            #region Init Cubism Live2D Options
            o_l2dMotionMode = new GroupedOption<CubismLive2DExtractor.Live2DMotionMode>
            (
                optionDefaultValue: CubismLive2DExtractor.Live2DMotionMode.MonoBehaviour,
                optionName: "--l2d-motion-mode <value>",
                optionDescription: "Specify Live2D motion export mode\n" +
                    "<Value: monoBehaviour(default) | animationClip>\n" +
                    "MonoBehaviour - Try to export motions from MonoBehaviour Fade motions\n" +
                    "If no Fade motions are found, the AnimationClip method will be used\n" +
                    "AnimationClip - Try to export motions using AnimationClip assets\n",
                optionExample: "Example: \"--l2d-motion-mode animationClip\"\n",
                optionHelpGroup: HelpGroups.Live2D
            );
            f_l2dForceBezier = new GroupedOption<bool>
            (
                optionDefaultValue: false,
                optionName: "--l2d-force-bezier",
                optionDescription: "(Flag) If specified, Linear motion segments will be calculated as Bezier segments\n" +
                    "(May help if the exported motions look jerky/not smooth enough)",
                optionExample: "",
                optionHelpGroup: HelpGroups.Live2D,
                isFlag: true
            );
            #endregion

            #region Init FBX Options
            o_fbxScaleFactor = new GroupedOption<float>
            (
                optionDefaultValue: 1f,
                optionName: "--fbx-scale-factor <value>",
                optionDescription: "Specify the FBX Scale Factor\n" +
                    "<Value: float number from 0 to 100 (default=1)>\n",
                optionExample: "Example: \"--fbx-scale-factor 50\"\n",
                optionHelpGroup: HelpGroups.FBX
            );
            o_fbxBoneSize = new GroupedOption<int>
            (
                optionDefaultValue: 10,
                optionName: "--fbx-bone-size <value>",
                optionDescription: "Specify the FBX Bone Size\n" +
                    "<Value: integer number from 0 to 100 (default=10)>\n",
                optionExample: "Example: \"--fbx-bone-size 10\"",
                optionHelpGroup: HelpGroups.FBX
            );
            #endregion

            #region Init Filter Options
            o_filterByName = new GroupedOption<List<string>>
            (
                optionDefaultValue: new List<string>(),
                optionName: "--filter-by-name <text>",
                optionDescription: "Specify the name by which assets should be filtered\n" +
                    "*To specify multiple names write them separated by ',' or ';' without spaces\n",
                optionExample: "Example: \"--filter-by-name char\" or \"--filter-by-name char,bg\"\n",
                optionHelpGroup: HelpGroups.Filter
            );
            o_filterByContainer = new GroupedOption<List<string>>
            (
                optionDefaultValue: new List<string>(),
                optionName: "--filter-by-container <text>",
                optionDescription: "Specify the container by which assets should be filtered\n" +
                    "*To specify multiple containers write them separated by ',' or ';' without spaces\n",
                optionExample: "Example: \"--filter-by-container arts\" or \"--filter-by-container arts,icons\"\n",
                optionHelpGroup: HelpGroups.Filter
            );
            o_filterByPathID = new GroupedOption<List<string>>
            (
                optionDefaultValue: new List<string>(),
                optionName: "--filter-by-pathid <text>",
                optionDescription: "Specify the PathID by which assets should be filtered\n" +
                    "*To specify multiple PathIDs write them separated by ',' or ';' without spaces\n",
                optionExample: "Example: \"--filter-by-pathid 7238605633795851352,-2430306240205277265\"\n",
                optionHelpGroup: HelpGroups.Filter
            );
            o_filterByText = new GroupedOption<List<string>>
            (
                optionDefaultValue: new List<string>(),
                optionName: "--filter-by-text <text>",
                optionDescription: "Specify the text by which assets should be filtered\n" +
                    "Looks for assets that contain the specified text in their names or containers\n" +
                    "*To specify multiple values write them separated by ',' or ';' without spaces\n",
                optionExample: "Example: \"--filter-by-text portrait\" or \"--filter-by-text portrait,art\"\n",
                optionHelpGroup: HelpGroups.Filter
            );
            #endregion

            #region Init Advanced Options
            o_customCompressionType = new GroupedOption<CustomCompressionType>
            (
                optionDefaultValue: CustomCompressionType.Zstd,
                optionName: "--custom-compression <value>",
                optionDescription: "Specify the compression type for assets that use custom compression\n" + 
                    "<Value: zstd(default) | lz4>\n" +
                    "Zstd - Try to decompress as zstd archive\n" +
                    "Lz4 - Try to decompress as lz4 archive\n",
                optionExample: "Example: \"--custom-compression lz4\"\n",
                optionHelpGroup: HelpGroups.Advanced
            );

            o_maxParallelExportTasks = new GroupedOption<int>
            (
                optionDefaultValue: Environment.ProcessorCount - 1,
                optionName: "--max-export-tasks <value>",
                optionDescription: "Specify the number of parallel tasks for asset export\n" +
                    "<Value: integer number from 1 to max number of cores (default=max)>\n" +
                    "Max - Number of cores in your CPU\n",
                optionExample: "Example: \"--max-export-tasks 8\"\n",
                optionHelpGroup: HelpGroups.Advanced
            );

            o_exportAssetList = new GroupedOption<ExportListType>
            (
                optionDefaultValue: ExportListType.None,
                optionName: "--export-asset-list <value>",
                optionDescription: "Specify the format in which you want to export asset list\n" +
                    "<Value: none(default) | xml>\n" +
                    "None - Do not export asset list\n",
                optionExample: "Example: \"--export-asset-list xml\"\n",
                optionHelpGroup: HelpGroups.Advanced
            );
            o_assemblyPath = new GroupedOption<string>
            (
                optionDefaultValue: "",
                optionName: "--assembly-folder <path>",
                optionDescription: "Specify the path to the assembly folder\n",
                optionExample: "",
                optionHelpGroup: HelpGroups.Advanced
            );
            o_unityVersion = new GroupedOption<UnityVersion>
            (
                optionDefaultValue: null,
                optionName: "--unity-version <text>",
                optionDescription: "Specify Unity version\n",
                optionExample: "Example: \"--unity-version 2017.4.39f1\"\n",
                optionHelpGroup: HelpGroups.Advanced
            );
            f_notRestoreExtensionName = new GroupedOption<bool>
            (
                optionDefaultValue: false,
                optionName: "--not-restore-extension",
                optionDescription: "(Flag) If specified, AssetStudio will not try to use/restore original TextAsset\nextension name, and will just export all TextAssets with the \".txt\" extension\n",
                optionExample: "",
                optionHelpGroup: HelpGroups.Advanced,
                isFlag: true
            );
            f_avoidLoadingViaTypetree = new GroupedOption<bool>
            (
                optionDefaultValue: false,
                optionName: "--avoid-typetree-loading",
                optionDescription: "(Flag) If specified, AssetStudio will not try to parse assets at load time\nusing their type tree\n",
                optionExample: "",
                optionHelpGroup: HelpGroups.Advanced,
                isFlag: true
            );
            f_loadAllAssets = new GroupedOption<bool>
            (
                optionDefaultValue: false,
                optionName: "--load-all",
                optionDescription: "(Flag) If specified, AssetStudio will load assets of all types\n(Only for Dump, Info and ExportRaw modes)",
                optionExample: "",
                optionHelpGroup: HelpGroups.Advanced,
                isFlag: true
            );
            #endregion
        }

        public static void ParseArgs(string[] args)
        {
            cliArgs = args;

            var brightYellow = ColorConsole.BrightYellow;
            var brightRed = ColorConsole.BrightRed;

            if (args.Length == 0 || args.Any(x => x.ToLower() == "-h" || x.ToLower() == "--help" || x.ToLower() == "-?"))
            {
                showHelp = true;
                return;
            }

            if (!args[0].StartsWith("-"))
            {
                inputPath = Path.GetFullPath(args[0]).Replace("\"", "");
                if (!Directory.Exists(inputPath) && !File.Exists(inputPath))
                {
                    Console.WriteLine($"{"Error:".Color(brightRed)} Invalid input path \"{args[0].Color(brightRed)}\".\n" +
                        $"Specified file or folder was not found. The input path must be specified as the first argument.");
                    return;
                }
            }
            else
            {
                Console.WriteLine($"{"Error:".Color(brightRed)} Input path was empty. Specify the input path as the first argument.");
                return;
            }

            var resplittedArgs = new List<string>();
            for (int i = 1; i < args.Length; i++)
            {
                string arg = args[i];

                if (arg.Contains('='))
                {
                    var splittedArgs = arg.Split('=');
                    resplittedArgs.Add(splittedArgs[0]);
                    resplittedArgs.Add(splittedArgs[1]);
                }
                else
                {
                    resplittedArgs.Add(arg);
                }    
            };

            #region Parse "Working Mode" Option
            var workModeOptionIndex = resplittedArgs.FindIndex(x => x.ToLower() == "-m" || x.ToLower() == "--mode");
            if (workModeOptionIndex >= 0)
            {
                var option = resplittedArgs[workModeOptionIndex];
                if (workModeOptionIndex + 1 >= resplittedArgs.Count)
                {
                    Console.WriteLine($"{"Error during parsing options:".Color(brightRed)} Value for [{option.Color(brightYellow)}] option was not found.\n");
                    TryFindOptionDescription(option, optionsDict);
                    return;
                }
                var value = resplittedArgs[workModeOptionIndex + 1];
                switch (value.ToLower())
                {
                    case "export":
                        o_workMode.Value = WorkMode.Export;
                        break;
                    case "raw":
                    case "exportraw":
                        o_workMode.Value = WorkMode.ExportRaw;
                        break;
                    case "dump":
                        o_workMode.Value = WorkMode.Dump;
                        break;
                    case "info":
                        o_workMode.Value = WorkMode.Info;
                        break;
                    case "l2d":
                    case "live2d":
                        o_workMode.Value = WorkMode.Live2D;
                        o_exportAssetTypes.Value = new List<ClassIDType>()
                        {
                            ClassIDType.AnimationClip,
                            ClassIDType.MonoBehaviour,
                            ClassIDType.Texture2D,
                        };
                        break;
                    case "splitobjects":
                        o_workMode.Value = WorkMode.SplitObjects;
                        o_exportAssetTypes.Value = new List<ClassIDType>()
                        {
                            ClassIDType.Texture2D,
                            ClassIDType.Material,
                            ClassIDType.Mesh,
                            ClassIDType.MeshRenderer,
                            ClassIDType.MeshFilter,
                        };
                        break;
                    default:
                        Console.WriteLine($"{"Error".Color(brightRed)} during parsing [{option.Color(brightYellow)}] option. Unsupported working mode: [{value.Color(brightRed)}].\n");
                        ShowOptionDescription(o_workMode);
                        return;
                }
                resplittedArgs.RemoveRange(workModeOptionIndex, 2);
            }
            #endregion

            #region Parse Flags
            for (int i = 0; i < resplittedArgs.Count; i++) 
            {
                string flag = resplittedArgs[i].ToLower();

                switch(flag)
                {
                    case "--l2d-force-bezier":
                        if (o_workMode.Value != WorkMode.Live2D)
                        {
                            Console.WriteLine($"{"Error".Color(brightRed)} during parsing [{flag.Color(brightYellow)}] flag. This flag is not suitable for the current working mode [{o_workMode.Value}].\n");
                            ShowOptionDescription(o_workMode);
                            return;
                        }
                        f_l2dForceBezier.Value = true;
                        resplittedArgs.RemoveAt(i);
                        break;
                    case "--not-restore-extension":
                        f_notRestoreExtensionName.Value = true;
                        resplittedArgs.RemoveAt(i);
                        break;
                    case "--avoid-typetree-loading":
                        f_avoidLoadingViaTypetree.Value = true;
                        resplittedArgs.RemoveAt(i);
                        break;
                    case "--load-all":
                        switch (o_workMode.Value)
                        {
                            case WorkMode.ExportRaw:
                            case WorkMode.Dump:
                            case WorkMode.Info:
                                f_loadAllAssets.Value = true;
                                resplittedArgs.RemoveAt(i);
                                break;
                            default:
                                Console.WriteLine($"{"Error".Color(brightRed)} during parsing [{flag.Color(brightYellow)}] flag. This flag is not suitable for the current working mode [{o_workMode.Value}].\n");
                                ShowOptionDescription(f_loadAllAssets, isFlag: true);
                                return;
                        }
                        break;
                }
            }            
            #endregion

            #region Parse Options
            for (int i = 0; i < resplittedArgs.Count; i++)
            {
                var option = resplittedArgs[i].ToLower();
                try
                {
                    var value = resplittedArgs[i + 1].Replace("\"", "");
                    switch (option)
                    {
                        case "-t":
                        case "--asset-type":
                            if (o_workMode.Value == WorkMode.Live2D || o_workMode.Value == WorkMode.SplitObjects)
                            {
                                i++;
                                continue;
                            }
                            var splittedTypes = ValueSplitter(value);
                            o_exportAssetTypes.Value = new List<ClassIDType>();
                            foreach (var type in splittedTypes)
                            {
                                switch (type.ToLower())
                                {
                                    case "tex2d":
                                        o_exportAssetTypes.Value.Add(ClassIDType.Texture2D);
                                        break;
                                    case "tex2darray":
                                        o_exportAssetTypes.Value.Add(ClassIDType.Texture2DArray);
                                        break;
                                    case "audio":
                                        o_exportAssetTypes.Value.Add(ClassIDType.AudioClip);
                                        break;
                                    case "video":
                                        o_exportAssetTypes.Value.Add(ClassIDType.VideoClip);
                                        break;
                                    case "all":
                                        o_exportAssetTypes.Value = exportableAssetTypes;
                                        break;
                                    default:
                                        var isKnownType = knownAssetTypesDict.TryGetValue(type.ToLower(), out var assetType);
                                        if (isKnownType)
                                        {
                                            if (f_loadAllAssets.Value || exportableAssetTypes.Contains(assetType))
                                            {
                                                o_exportAssetTypes.Value.Add(assetType);
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine($"{"Error".Color(brightRed)} during parsing [{option.Color(brightYellow)}] option. Unknown asset type specified [{type.Color(brightRed)}].\n");
                                            ShowOptionDescription(o_exportAssetTypes);
                                            return;
                                        }
                                        Console.WriteLine($"{"Error".Color(brightRed)} during parsing [{option.Color(brightYellow)}] option. Asset type [{type.Color(brightRed)}] is not supported for exporting.\n");
                                        ShowOptionDescription(o_exportAssetTypes);
                                        return;
                                }
                            }
                            break;
                        case "-g":
                        case "--group-option":
                            switch (value.ToLower())
                            {
                                case "type":
                                    o_groupAssetsBy.Value = AssetGroupOption.TypeName;
                                    break;
                                case "container":
                                    o_groupAssetsBy.Value = AssetGroupOption.ContainerPath;
                                    break;
                                case "containerfull":
                                    o_groupAssetsBy.Value = AssetGroupOption.ContainerPathFull;
                                    break;
                                case "filename":
                                    o_groupAssetsBy.Value = AssetGroupOption.SourceFileName;
                                    break;
                                case "scenehierarchy":
                                    o_groupAssetsBy.Value = AssetGroupOption.SceneHierarchy;
                                    break;
                                case "none":
                                    o_groupAssetsBy.Value = AssetGroupOption.None;
                                    break;
                                default:
                                    Console.WriteLine($"{"Error".Color(brightRed)} during parsing [{option.Color(brightYellow)}] option. Unsupported grouping option: [{value.Color(brightRed)}].\n");
                                    ShowOptionDescription(o_groupAssetsBy);
                                    return;
                            }
                            break;
                        case "-f":
                        case "--filename-format":
                            switch (value.ToLower())
                            {
                                case "assetname":
                                    o_filenameFormat.Value = FilenameFormat.AssetName;
                                    break;
                                case "assetname_pathid":
                                    o_filenameFormat.Value = FilenameFormat.AssetName_PathID;
                                    break;
                                case "pathid":
                                    o_filenameFormat.Value = FilenameFormat.PathID;
                                    break;
                                default:
                                    Console.WriteLine($"{"Error".Color(brightRed)} during parsing [{option.Color(brightYellow)}] option. Unsupported file name format: [{value.Color(brightRed)}].\n");
                                    ShowOptionDescription(o_filenameFormat);
                                    return;
                            }
                            break;
                        case "-o":
                        case "--output":
                            try
                            {
                                value = Path.GetFullPath(value);
                                if (!Directory.Exists(value))
                                {
                                    Directory.CreateDirectory(value);
                                }
                                if (!value.EndsWith($"{Path.DirectorySeparatorChar}"))
                                {
                                    value += Path.DirectorySeparatorChar;
                                }
                                o_outputFolder.Value = value;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"{"Warning:".Color(brightYellow)} Invalid output folder \"{value.Color(brightYellow)}\".\n{ex.Message}");
                                Console.WriteLine($"Working folder \"{o_outputFolder.Value.Color(brightYellow)}\" will be used as the output folder.\n");
                                Console.WriteLine("Press ESC to exit or any other key to continue...\n");
                                switch (Console.ReadKey(intercept: true).Key)
                                {
                                    case ConsoleKey.Escape:
                                        return;
                                }
                            }
                            break;
                        case "--log-level":
                            switch (value.ToLower())
                            {
                                case "verbose":
                                    o_logLevel.Value = LoggerEvent.Verbose;
                                    break;
                                case "debug":
                                    o_logLevel.Value = LoggerEvent.Debug;
                                    break;
                                case "info":
                                    o_logLevel.Value = LoggerEvent.Info;
                                    break;
                                case "warning":
                                    o_logLevel.Value = LoggerEvent.Warning;
                                    break;
                                case "error":
                                    o_logLevel.Value = LoggerEvent.Error;
                                    break;
                                default:
                                    Console.WriteLine($"{"Error".Color(brightRed)} during parsing [{option.Color(brightYellow)}] option. Unsupported log level value: [{value.Color(brightRed)}].\n");
                                    ShowOptionDescription(o_logLevel);
                                    return;
                            }
                            break;
                        case "--log-output":
                            switch (value.ToLower())
                            {
                                case "console":
                                    o_logOutput.Value = LogOutputMode.Console;
                                    break;
                                case "file":
                                    o_logOutput.Value = LogOutputMode.File;
                                    break;
                                case "both":
                                    o_logOutput.Value = LogOutputMode.Both;
                                    break;
                                default:
                                    Console.WriteLine($"{"Error".Color(brightRed)} during parsing [{option.Color(brightYellow)}] option. Unsupported log output mode: [{value.Color(brightRed)}].\n");
                                    ShowOptionDescription(o_logOutput);
                                    return;
                            }
                            break;
                        case "--image-format":
                            switch (value.ToLower())
                            {
                                case "jpg":
                                case "jpeg":
                                    o_imageFormat.Value = ImageFormat.Jpeg;
                                    break;
                                case "png":
                                    o_imageFormat.Value = ImageFormat.Png;
                                    break;
                                case "bmp":
                                    o_imageFormat.Value = ImageFormat.Bmp;
                                    break;
                                case "tga":
                                    o_imageFormat.Value = ImageFormat.Tga;
                                    break;
                                case "webp":
                                    o_imageFormat.Value = ImageFormat.Webp;
                                    break;
                                case "none":
                                    convertTexture = false;
                                    break;
                                default:
                                    Console.WriteLine($"{"Error".Color(brightRed)} during parsing [{option.Color(brightYellow)}] option. Unsupported image format: [{value.Color(brightRed)}].\n");
                                    ShowOptionDescription(o_imageFormat);
                                    return;
                            }
                            break;
                        case "--audio-format":
                            switch (value.ToLower())
                            {
                                case "wav":
                                case "wave":
                                    o_audioFormat.Value = AudioFormat.Wav;
                                    break;
                                case "none":
                                    o_audioFormat.Value = AudioFormat.None;
                                    break;
                                default:
                                    Console.WriteLine($"{"Error".Color(brightRed)} during parsing [{option.Color(brightYellow)}] option. Unsupported audio format: [{value.Color(brightRed)}].\n");
                                    ShowOptionDescription(o_audioFormat);
                                    return;
                            }
                            break;
                        case "--l2d-motion-mode":
                            if (o_workMode.Value != WorkMode.Live2D)
                            {
                                Console.WriteLine($"{"Error".Color(brightRed)} during parsing [{option.Color(brightYellow)}] option. This option is not suitable for the current working mode [{o_workMode.Value}].\n");
                                ShowOptionDescription(o_workMode);
                                return;
                            }
                            switch (value.ToLower())
                            {
                                case "fade":
                                case "monobehaviour":
                                    o_l2dMotionMode.Value = CubismLive2DExtractor.Live2DMotionMode.MonoBehaviour;
                                    break;
                                case "animationclip":
                                case "animationclipv2":
                                    o_l2dMotionMode.Value = CubismLive2DExtractor.Live2DMotionMode.AnimationClipV2;
                                    break;
                                case "animationclipv1":
                                    o_l2dMotionMode.Value = CubismLive2DExtractor.Live2DMotionMode.AnimationClipV1;
                                    break;
                                default:
                                    Console.WriteLine($"{"Error".Color(brightRed)} during parsing [{option.Color(brightYellow)}] option. Unsupported Live2D motion mode: [{value.Color(brightRed)}].\n");
                                    ShowOptionDescription(o_l2dMotionMode);
                                    return;
                            }
                            break;
                        case "--fbx-scale-factor":
                        {
                            var isFloat = float.TryParse(value, out var floatValue);
                            if (isFloat && floatValue >= 0 && floatValue <= 100)
                            {
                                o_fbxScaleFactor.Value = floatValue;
                            }
                            else
                            {
                                Console.WriteLine($"{"Error".Color(brightRed)} during parsing [{option.Color(brightYellow)}] option. Unsupported scale factor value: [{value.Color(brightRed)}].\n");
                                ShowOptionDescription(o_fbxScaleFactor);
                                return;
                            }
                            break;
                        }
                        case "--fbx-bone-size":
                        {
                            var isInt = int.TryParse(value, out var intValue);
                            if (isInt && intValue >= 0 && intValue <= 100)
                            {
                                o_fbxBoneSize.Value = intValue;
                            }
                            else
                            {
                                Console.WriteLine($"{"Error".Color(brightRed)} during parsing [{option.Color(brightYellow)}] option. Unsupported bone size value: [{value.Color(brightRed)}].\n");
                                ShowOptionDescription(o_fbxBoneSize);
                                return;
                            }
                            break;
                        }
                        case "--custom-compression":
                            switch (value.ToLower())
                            {
                                case "zstd":
                                    o_customCompressionType.Value = CustomCompressionType.Zstd;
                                    break;
                                case "lz4":
                                case "lz4hc":
                                    o_customCompressionType.Value = CustomCompressionType.Lz4;
                                    break;
                                default:
                                    Console.WriteLine($"{"Error".Color(brightRed)} during parsing [{option.Color(brightYellow)}] option. Unsupported compression type: [{value.Color(brightRed)}].\n");
                                    ShowOptionDescription(o_customCompressionType);
                                    return;
                            }
                            break;
                        case "--max-export-tasks":
                        {
                            var processorCount = Environment.ProcessorCount;
                            if (value.ToLower() == "max")
                            {
                                o_maxParallelExportTasks.Value = processorCount - 1;
                            }
                            else
                            {
                                var isInt = int.TryParse(value, out var intValue);
                                if (isInt && intValue >= 0 && intValue <= processorCount)
                                {
                                    o_maxParallelExportTasks.Value = Math.Min(intValue, processorCount - 1);
                                }
                                else
                                {
                                    Console.WriteLine($"{"Error".Color(brightRed)} during parsing [{option.Color(brightYellow)}] option. Unsupported number of parallel tasks: [{value.Color(brightRed)}].\n");
                                    ShowOptionDescription(o_maxParallelExportTasks);
                                    return;
                                }
                            }
                            break;
                        }
                        case "--export-asset-list":
                            switch (value.ToLower())
                            {
                                case "xml":
                                    o_exportAssetList.Value = ExportListType.XML;
                                    break;
                                case "none":
                                    o_exportAssetList.Value = ExportListType.None;
                                    break;
                                default:
                                    Console.WriteLine($"{"Error".Color(brightRed)} during parsing [{option.Color(brightYellow)}] option. Unsupported asset list export option: [{value.Color(brightRed)}].\n");
                                    ShowOptionDescription(o_exportAssetList);
                                    return;
                            }
                            break;
                        case "--filter-by-name":
                            o_filterByName.Value.AddRange(ValueSplitter(value));
                            filterBy = filterBy == FilterBy.None ? FilterBy.Name : filterBy == FilterBy.Container ? FilterBy.NameAndContainer : filterBy;
                            break;
                        case "--filter-by-container":
                            o_filterByContainer.Value.AddRange(ValueSplitter(value));
                            filterBy = filterBy == FilterBy.None ? FilterBy.Container : filterBy == FilterBy.Name ? FilterBy.NameAndContainer : filterBy;
                            break;
                        case "--filter-by-pathid":
                            o_filterByPathID.Value.AddRange(ValueSplitter(value));
                            filterBy = FilterBy.PathID;
                            break;
                        case "--filter-by-text":
                            o_filterByText.Value.AddRange(ValueSplitter(value));
                            filterBy = FilterBy.NameOrContainer;
                            break;
                        case "--assembly-folder":
                            if (Directory.Exists(value))
                            {
                                o_assemblyPath.Value = value;
                                Studio.assemblyLoader.Load(value);
                            }
                            else
                            {
                                Console.WriteLine($"{"Error".Color(brightRed)} during parsing [{option.Color(brightYellow)}] option. Assembly folder [{value.Color(brightRed)}] was not found.");
                                return;
                            }
                            break;
                        case "--unity-version":
                            try
                            {
                                o_unityVersion.Value = new UnityVersion(value);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"{"Error".Color(brightRed)} during parsing [{option.Color(brightYellow)}] option with value [{value.Color(brightRed)}].\n{e.Message}\n");
                                ShowOptionDescription(o_unityVersion);
                                return;
                            }
                            break;
                        default:
                            Console.WriteLine($"{"Error:".Color(brightRed)} Unknown option [{option.Color(brightRed)}].\n");
                            if (!TryFindOptionDescription(option, optionsDict))
                            {
                                TryFindOptionDescription(option, flagsDict, isFlag: true);
                            }
                            return;
                    }
                    i++;
                }
                catch (ArgumentOutOfRangeException)
                {
                    if (optionsDict.Any(x => x.Key.Contains(option)))
                    {
                        Console.WriteLine($"{"Error during parsing options:".Color(brightRed)} Value for [{option.Color(brightYellow)}] option was not found.\n");
                        TryFindOptionDescription(option, optionsDict);
                    }
                    else if (flagsDict.Any(x => x.Key.Contains(option)))
                    {
                        Console.WriteLine($"{"Error:".Color(brightRed)} Unknown flag [{option.Color(brightRed)}].\n");
                        TryFindOptionDescription(option, flagsDict, isFlag: true);
                    }
                    else
                    {
                        Console.WriteLine($"{"Error:".Color(brightRed)} Unknown option [{option.Color(brightRed)}].");
                    }
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unknown Error.".Color(ColorConsole.Red));
                    Console.WriteLine(ex);
                    return;
                }
            }
            #endregion

            if (!Studio.assemblyLoader.Loaded)
            {
                Studio.assemblyLoader.Loaded = true;
            }
            if (o_outputFolder.Value == o_outputFolder.DefaultValue)
            {
                var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, o_outputFolder.DefaultValue + Path.DirectorySeparatorChar);
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                o_outputFolder.Value = fullPath;
            }
            isParsed = true;
        }

        private static string[] ValueSplitter(string value)
        {
            var separator = value.Contains(';') ? ';' : ',';
            return value.Split(separator);
        }

        private static void ShowOptionDescription<T>(Option<T> option, bool isFlag = false)
        {
            var arg = isFlag ? "Flag" : "Option";
            var optionDesc = option.Description + option.Example.Color(ColorConsole.BrightBlack);
            Console.WriteLine($"{arg} description:\n{optionDesc}");
        }

        private static bool TryFindOptionDescription(string option, Dictionary<string, string> dict, bool isFlag = false)
        {
            var optionDesc = dict.Where(x => x.Key.Contains(option)).ToArray();
            if (optionDesc.Length != 0)
            {
                var arg = isFlag ? "flag" : "option";
                var rand = new Random();
                var rndOption = optionDesc.ElementAt(rand.Next(0, optionDesc.Length));
                Console.WriteLine($"Did you mean [{$"{rndOption.Key}".Color(ColorConsole.BrightYellow)}] {arg}?");
                Console.WriteLine($"Here's a description of it: \n\n{rndOption.Value}");

                return true;
            }
            return false;
        }

        public static void ShowHelp(bool showUsageOnly = false)
        {
            const int indent = 22;
            var helpMessage = new StringBuilder();
            var usage = new StringBuilder();
            var appAssembly = typeof(Program).Assembly.GetName();
            usage.Append($"{"Usage:".Color(ColorConsole.BrightYellow)} {appAssembly.Name} <input path to asset file/folder> ");

            var i = 0;
            foreach (var optionsGroup in optionGroups.Keys)
            {
                helpMessage.AppendLine($"{optionsGroup} Options:".Color(ColorConsole.BrightYellow));
                foreach (var optionDict in optionGroups[optionsGroup])
                {
                    var optionName = $"{optionDict.Key,-indent - 8}";
                    var optionDesc = optionDict.Value.Replace("\n", $"{"\n",-indent - 11}");
                    helpMessage.AppendLine($"  {optionName}{optionDesc}");

                    usage.Append($"[{optionDict.Key}] ");
                    if (i++ % 2 == 0)
                    {
                        usage.Append($"\n{"",indent}");
                    }
                }
                helpMessage.AppendLine();
            }

            if (showUsageOnly)
            {
                Console.WriteLine(usage);
            }
            else
            {
                var arch = Environment.Is64BitProcess ? "x64" : "x32";
                Console.WriteLine($"# {appAssembly.Name} [{arch}]\n# Based on AssetStudioMod v{appAssembly.Version}\n");
                Console.WriteLine($"{usage}\n\n{helpMessage}");
            }
        }

        private static string ShowCurrentFilter()
        {
            switch (filterBy)
            {
                case FilterBy.Name:
                    return $"# Filter by {filterBy}(s): \"{string.Join("\", \"", o_filterByName.Value)}\"";
                case FilterBy.Container:
                    return $"# Filter by {filterBy}(s): \"{string.Join("\", \"", o_filterByContainer.Value)}\"";
                case FilterBy.PathID:
                    return $"# Filter by {filterBy}(s): \"{string.Join("\", \"", o_filterByPathID.Value)}\"";
                case FilterBy.NameOrContainer:
                    return $"# Filter by Text: \"{string.Join("\", \"", o_filterByText.Value)}\"";
                case FilterBy.NameAndContainer:
                    return $"# Filter by Name(s): \"{string.Join("\", \"", o_filterByName.Value)}\"\n# Filter by Container(s): \"{string.Join("\", \"", o_filterByContainer.Value)}\"";
                default:
                    return $"# Filter by: {filterBy}";
            }
        }

        private static string ShowExportTypes()
        {
            switch (o_workMode.Value)
            {
                case WorkMode.ExportRaw:
                case WorkMode.Dump:
                case WorkMode.Info:
                    return f_loadAllAssets.Value && o_exportAssetTypes.Value == o_exportAssetTypes.DefaultValue
                        ? $"# Export Asset Type(s): All"
                        : $"# Export Asset Type(s): {string.Join(", ", o_exportAssetTypes.Value)}";
                default:
                    return $"# Export Asset Type(s): {string.Join(", ", o_exportAssetTypes.Value)}";
            }
        }

        public static void ShowCurrentOptions()
        {
            var sb = new StringBuilder();
            sb.AppendLine("[Current Options]");
            sb.AppendLine($"# Working Mode: {o_workMode}");
            if (o_customCompressionType.Value != o_customCompressionType.DefaultValue)
            {
                sb.AppendLine($"# Custom Compression Type: {o_customCompressionType}");
            }
            sb.AppendLine($"# Parse Assets Using TypeTree: {!f_avoidLoadingViaTypetree.Value}");
            sb.AppendLine($"# Input Path: \"{inputPath}\"");
            switch (o_workMode.Value)
            {
                case WorkMode.Export:
                case WorkMode.ExportRaw:
                case WorkMode.Dump:
                    sb.AppendLine($"# Output Path: \"{o_outputFolder}\"");
                    if (o_workMode.Value != WorkMode.Export)
                    {
                        sb.AppendLine($"# Load All Assets: {f_loadAllAssets}");
                    }
                    sb.AppendLine(ShowExportTypes());
                    sb.AppendLine($"# Asset Group Option: {o_groupAssetsBy}");
                    sb.AppendLine($"# Filename format: {o_filenameFormat}");
                    if (o_workMode.Value == WorkMode.Export)
                    {
                        sb.AppendLine($"# Export Image Format: {o_imageFormat}");
                        sb.AppendLine($"# Export Audio Format: {o_audioFormat}");
                    }
                    sb.AppendLine($"# Log Level: {o_logLevel}");
                    sb.AppendLine($"# Log Output: {o_logOutput}");
                    sb.AppendLine($"# Export Asset List: {o_exportAssetList}");
                    sb.AppendLine(ShowCurrentFilter());
                    sb.AppendLine($"# Assembly Path: \"{o_assemblyPath}\"");
                    sb.AppendLine($"# Unity Version: \"{o_unityVersion}\"");
                    if (o_workMode.Value == WorkMode.Export)
                    {
                        sb.AppendLine($"# Max Parallel Export Tasks: {o_maxParallelExportTasks}");
                        sb.AppendLine($"# Restore TextAsset Extension: {!f_notRestoreExtensionName.Value}");
                    }
                    break;
                case WorkMode.Info:
                    sb.AppendLine($"# Load All Assets: {f_loadAllAssets}");
                    sb.AppendLine(ShowExportTypes());
                    sb.AppendLine($"# Log Level: {o_logLevel}");
                    sb.AppendLine($"# Log Output: {o_logOutput}");
                    sb.AppendLine($"# Export Asset List: {o_exportAssetList}");
                    sb.AppendLine(ShowCurrentFilter());
                    sb.AppendLine($"# Unity Version: \"{o_unityVersion}\"");
                    break;
                case WorkMode.Live2D:
                case WorkMode.SplitObjects:
                    sb.AppendLine($"# Output Path: \"{o_outputFolder}\"");
                    sb.AppendLine($"# Log Level: {o_logLevel}");
                    sb.AppendLine($"# Log Output: {o_logOutput}");
                    sb.AppendLine($"# Export Asset List: {o_exportAssetList}");
                    if (o_workMode.Value == WorkMode.SplitObjects)
                    {
                        sb.AppendLine($"# Export Image Format: {o_imageFormat}");
                        sb.AppendLine($"# Filter by Name(s): \"{string.Join("\", \"", o_filterByName.Value)}\"");
                    }
                    else
                    {
                        sb.AppendLine($"# Live2D Motion Export Method: {o_l2dMotionMode}");
                        sb.AppendLine($"# Force Bezier: {f_l2dForceBezier }");
                        sb.AppendLine($"# Assembly Path: \"{o_assemblyPath}\"");
                    }
                    sb.AppendLine($"# Unity Version: \"{o_unityVersion}\"");
                    break;
            }
            sb.AppendLine("======");
            Logger.Info(sb.ToString());
        }
    }
}
