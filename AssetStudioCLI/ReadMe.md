## AssetStudioModCLI
CLI version of AssetStudioMod.
- Supported asset types for export: `Texture2D`, `Texture2DArray`, `Sprite`, `TextAsset`, `MonoBehaviour`, `Font`, `Shader`, `MovieTexture`, `AudioClip`, `VideoClip`, `Mesh`.
- *There are no plans to add support for `AnimationClip`, `Animator` for now.*

### Usage
```
AssetStudioModCLI <input path to asset file/folder> [-m, --mode <value>]
                      [-t, --asset-type <value(s)>] [-g, --group-option <value>]
                      [-f, --filename-format <value>] [-o, --output <path>]
                      [-h, --help] [--log-level <value>]
                      [--log-output <value>] [--image-format <value>]
                      [--audio-format <value>] [--l2d-motion-mode <value>]
                      [--l2d-force-bezier] [--fbx-scale-factor <value>]
                      [--fbx-bone-size <value>] [--filter-by-name <text>]
                      [--filter-by-container <text>] [--filter-by-pathid <text>]
                      [--filter-by-text <text>] [--custom-compression <value>]
                      [--max-export-tasks <value>] [--export-asset-list <value>]
                      [--assembly-folder <path>] [--unity-version <text>]
                      [--not-restore-extension] [--avoid-typetree-loading]
                      [--load-all]

General Options:
  -m, --mode <value>            Specify working mode
                                <Value: export(default) | exportRaw | dump | info | live2d | splitObjects>
                                Export - Exports converted assets
                                ExportRaw - Exports raw data
                                Dump - Makes asset dumps
                                Info - Loads file(s), shows the number of available for export assets and exits
                                Live2D - Exports Live2D Cubism models
                                SplitObjects - Exports split objects (fbx)
                                Example: "-m info"

  -t, --asset-type <value(s)>   Specify asset type(s) to export
                                <Value(s): tex2d, tex2dArray, sprite, textAsset, monoBehaviour, font, shader
                                movieTexture, audio, video, mesh | all(default)>
                                All - export all asset types, which are listed in the values
                                *To specify multiple asset types, write them separated by ',' or ';' without spaces
                                Examples: "-t sprite" or "-t tex2d,sprite,audio" or "-t tex2d;sprite;font"

  -g, --group-option <value>    Specify the way in which exported assets should be grouped
                                <Value: none | type | container(default) | containerFull | filename | sceneHierarchy>
                                None - Do not group exported assets
                                Type - Group exported assets by type name
                                Container - Group exported assets by container path
                                ContainerFull - Group exported assets by full container path (e.g. with prefab name)
                                SceneHierarchy - Group exported assets by their node path in scene hierarchy
                                Filename - Group exported assets by source file name
                                Example: "-g containerFull"

  -f, --filename-format <value> Specify the file name format for exported assets
                                <Value: assetName(default) | assetName_pathID | pathID>
                                AssetName - Asset file names will look like "assetName.extension"
                                AssetName_pathID - Asset file names will look like "assetName @pathID.extension"
                                PathID - Asset file names will look like "pathID.extension"
                                Example: "-f assetName_pathID"

  -o, --output <path>           Specify path to the output folder
                                If path isn't specified, 'ASExport' folder will be created in the program's work folder

  -h, --help                    Display help and exit

Logger Options:
  --log-level <value>           Specify the log level
                                <Value: verbose | debug | info(default) | warning | error>
                                Example: "--log-level warning"

  --log-output <value>          Specify the log output
                                <Value: console(default) | file | both>
                                Example: "--log-output both"

Convert Options:
  --image-format <value>        Specify the format for converting image assets
                                <Value: none | jpg | png(default) | bmp | tga | webp>
                                None - Do not convert images and export them as texture data (.tex)
                                Example: "--image-format jpg"

  --audio-format <value>        Specify the format for converting FMOD audio assets
                                <Value: none | wav(default)>
                                None - Do not convert fmod audios and export them in their own format
                                Example: "--audio-format wav"

Live2D Options:
  --l2d-motion-mode <value>     Specify Live2D motion export mode
                                <Value: monoBehaviour(default) | animationClip>
                                MonoBehaviour - Try to export motions from MonoBehaviour Fade motions
                                If no Fade motions are found, the AnimationClip method will be used
                                AnimationClip - Try to export motions using AnimationClip assets
                                Example: "--l2d-motion-mode animationClip"

  --l2d-force-bezier            (Flag) If specified, Linear motion segments will be calculated as Bezier segments
                                (May help if the exported motions look jerky/not smooth enough)

FBX Options:
  --fbx-scale-factor <value>    Specify the FBX Scale Factor
                                <Value: float number from 0 to 100 (default=1)>
                                Example: "--fbx-scale-factor 50"

  --fbx-bone-size <value>       Specify the FBX Bone Size
                                <Value: integer number from 0 to 100 (default=10)>
                                Example: "--fbx-bone-size 10"

Filter Options:
  --filter-by-name <text>       Specify the name by which assets should be filtered
                                *To specify multiple names write them separated by ',' or ';' without spaces
                                Example: "--filter-by-name char" or "--filter-by-name char,bg"

  --filter-by-container <text>  Specify the container by which assets should be filtered
                                *To specify multiple containers write them separated by ',' or ';' without spaces
                                Example: "--filter-by-container arts" or "--filter-by-container arts,icons"

  --filter-by-pathid <text>     Specify the PathID by which assets should be filtered
                                *To specify multiple PathIDs write them separated by ',' or ';' without spaces
                                Example: "--filter-by-pathid 7238605633795851352,-2430306240205277265"

  --filter-by-text <text>       Specify the text by which assets should be filtered
                                Looks for assets that contain the specified text in their names or containers
                                *To specify multiple values write them separated by ',' or ';' without spaces
                                Example: "--filter-by-text portrait" or "--filter-by-text portrait,art"


Advanced Options:
  --custom-compression <value>  Specify the compression type for assets that use custom compression
                                <Value: zstd(default) | lz4>
                                Zstd - Try to decompress as zstd archive
                                Lz4 - Try to decompress as lz4 archive
                                Example: "--custom-compression lz4"

  --max-export-tasks <value>    Specify the number of parallel tasks for asset export
                                <Value: integer number from 1 to max number of cores (default=max)>
                                Max - Number of cores in your CPU
                                Example: "--max-export-tasks 8"

  --export-asset-list <value>   Specify the format in which you want to export asset list
                                <Value: none(default) | xml>
                                None - Do not export asset list
                                Example: "--export-asset-list xml"

  --assembly-folder <path>      Specify the path to the assembly folder

  --unity-version <text>        Specify Unity version
                                Example: "--unity-version 2017.4.39f1"

  --not-restore-extension       (Flag) If specified, AssetStudio will not try to use/restore original TextAsset
                                extension name, and will just export all TextAssets with the ".txt" extension

  --avoid-typetree-loading      (Flag) If specified, AssetStudio will not try to parse assets at load time
                                using their type tree

  --load-all                    (Flag) If specified, AssetStudio will load assets of all types
                                (Only for Dump, Info and ExportRaw modes)
```
