////
// Based on UnityLive2DExtractor by Perfare
// https://github.com/Perfare/UnityLive2DExtractor
////

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssetStudio;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static CubismLive2DExtractor.CubismParsers;

namespace CubismLive2DExtractor
{
    public sealed class Live2DExtractor
    {
        private List<MonoBehaviour> Expressions { get; set; }
        private List<MonoBehaviour> FadeMotions { get; set; }
        private List<GameObject> GameObjects { get; set; }
        private List<AnimationClip> AnimationClips { get; set; }
        private List<Texture2D> Texture2Ds { get; set; }
        private HashSet<string> EyeBlinkParameters { get; set; }
        private HashSet<string> LipSyncParameters { get; set; }
        private HashSet<string> ParameterNames { get; set; }
        private HashSet<string> PartNames { get; set; }
        private MonoBehaviour MocMono { get; set; }
        private MonoBehaviour PhysicsMono { get; set; }
        private MonoBehaviour FadeMotionLst { get; set; }
        private List<MonoBehaviour> ParametersCdi { get; set; }
        private List<MonoBehaviour> PartsCdi { get; set; }

        public Live2DExtractor(IGrouping<string, AssetStudio.Object> assets, List<AnimationClip> inClipMotions = null, List<MonoBehaviour> inFadeMotions = null, MonoBehaviour inFadeMotionLst = null)
        {
            Expressions = new List<MonoBehaviour>();
            FadeMotions = inFadeMotions ?? new List<MonoBehaviour>();
            AnimationClips = inClipMotions ?? new List<AnimationClip>();
            GameObjects = new List<GameObject>();
            Texture2Ds = new List<Texture2D>();
            EyeBlinkParameters = new HashSet<string>();
            LipSyncParameters = new HashSet<string>();
            ParameterNames = new HashSet<string>();
            PartNames = new HashSet<string>();
            FadeMotionLst = inFadeMotionLst;
            ParametersCdi = new List<MonoBehaviour>();
            PartsCdi = new List<MonoBehaviour>();

            Logger.Debug("Sorting model assets..");
            foreach (var asset in assets)
            {
                switch (asset)
                {
                    case MonoBehaviour m_MonoBehaviour:
                        if (m_MonoBehaviour.m_Script.TryGet(out var m_Script))
                        {
                            switch (m_Script.m_ClassName)
                            {
                                case "CubismMoc":
                                    MocMono = m_MonoBehaviour;
                                    break;
                                case "CubismPhysicsController":
                                    PhysicsMono = m_MonoBehaviour;
                                    break;
                                case "CubismExpressionData":
                                    Expressions.Add(m_MonoBehaviour);
                                    break;
                                case "CubismFadeMotionData":
                                    if (inFadeMotions == null && inFadeMotionLst == null)
                                    {
                                        FadeMotions.Add(m_MonoBehaviour);
                                    }
                                    break;
                                case "CubismFadeMotionList":
                                    if (inFadeMotions == null && inFadeMotionLst == null)
                                    {
                                        FadeMotionLst = m_MonoBehaviour;
                                    }
                                    break;
                                case "CubismEyeBlinkParameter":
                                    if (m_MonoBehaviour.m_GameObject.TryGet(out var blinkGameObject))
                                    {
                                        EyeBlinkParameters.Add(blinkGameObject.m_Name);
                                    }
                                    break;
                                case "CubismMouthParameter":
                                    if (m_MonoBehaviour.m_GameObject.TryGet(out var mouthGameObject))
                                    {
                                        LipSyncParameters.Add(mouthGameObject.m_Name);
                                    }
                                    break;
                                case "CubismParameter":
                                    if (m_MonoBehaviour.m_GameObject.TryGet(out var paramGameObject))
                                    {
                                        ParameterNames.Add(paramGameObject.m_Name);
                                    }
                                    break;
                                case "CubismPart":
                                    if (m_MonoBehaviour.m_GameObject.TryGet(out var partGameObject))
                                    {
                                        PartNames.Add(partGameObject.m_Name);
                                    }
                                    break;
                                case "CubismDisplayInfoParameterName":
                                    if (m_MonoBehaviour.m_GameObject.TryGet(out _))
                                    {
                                        ParametersCdi.Add(m_MonoBehaviour);
                                    }
                                    break;
                                case "CubismDisplayInfoPartName":
                                    if (m_MonoBehaviour.m_GameObject.TryGet(out _))
                                    {
                                        PartsCdi.Add(m_MonoBehaviour);
                                    }
                                    break;
                            }
                        }
                        break;
                    case AnimationClip m_AnimationClip:
                        if (inClipMotions == null)
                        {
                            AnimationClips.Add(m_AnimationClip);
                        }
                        break;
                    case GameObject m_GameObject:
                        GameObjects.Add(m_GameObject);
                        break;
                    case Texture2D m_Texture2D:
                        Texture2Ds.Add(m_Texture2D);
                        break;
                }
            }
        }

        public void ExtractCubismModel(string destPath, string modelName, Live2DMotionMode motionMode, AssemblyLoader assemblyLoader, bool forceBezier = false, int parallelTaskCount = 1)
        {
            Directory.CreateDirectory(destPath);

            #region moc3
            using (var cubismModel = new CubismModel(MocMono))
            {
                var sb = new StringBuilder();
                sb.AppendLine("Model Stats:");
                sb.AppendLine($"SDK Version: {cubismModel.VersionDescription}");
                if (cubismModel.Version > 0)
                {
                    sb.AppendLine($"Canvas Width: {cubismModel.CanvasWidth}");
                    sb.AppendLine($"Canvas Height: {cubismModel.CanvasHeight}");
                    sb.AppendLine($"Center X: {cubismModel.CentralPosX}");
                    sb.AppendLine($"Center Y: {cubismModel.CentralPosY}");
                    sb.AppendLine($"Pixel Per Unit: {cubismModel.PixelPerUnit}");
                    sb.AppendLine($"Part Count: {cubismModel.PartCount}");
                    sb.AppendLine($"Parameter Count: {cubismModel.ParamCount}");
                    Logger.Debug(sb.ToString());

                    ParameterNames = cubismModel.ParamNames;
                    PartNames = cubismModel.PartNames;
                }
                cubismModel.SaveMoc3($"{destPath}{modelName}.moc3");
            }
            #endregion

            #region textures
            var textures = new SortedSet<string>();
            var destTexturePath = Path.Combine(destPath, "textures") + Path.DirectorySeparatorChar;

            if (Texture2Ds.Count == 0)
            {
                Logger.Warning($"No textures found for \"{modelName}\" model");
            }
            else
            {
                Directory.CreateDirectory(destTexturePath);
            }

            var textureBag = new ConcurrentBag<string>();
            var savePathHash = new ConcurrentDictionary<string, bool>();
            Parallel.ForEach(Texture2Ds, new ParallelOptions { MaxDegreeOfParallelism = parallelTaskCount }, texture2D =>
            {
                var savePath = $"{destTexturePath}{texture2D.m_Name}.png";
                if (!savePathHash.TryAdd(savePath, true))
                    return;

                using (var image = texture2D.ConvertToImage(flip: true))
                {
                    using (var file = File.OpenWrite(savePath))
                    {
                        image.WriteToStream(file, ImageFormat.Png);
                    }
                    textureBag.Add($"textures/{texture2D.m_Name}.png");
                }
            });
            textures.UnionWith(textureBag);
            #endregion

            #region physics3.json
            if (PhysicsMono != null)
            {
                var physicsDict = ParseMonoBehaviour(PhysicsMono, CubismMonoBehaviourType.Physics, assemblyLoader);
                if (physicsDict != null)
                {
                    try
                    {
                        var buff = ParsePhysics(physicsDict);
                        File.WriteAllText($"{destPath}{modelName}.physics3.json", buff);
                    }
                    catch (Exception e)
                    {
                        Logger.Warning($"Error in parsing physics data: {e.Message}");
                        PhysicsMono = null;
                    }
                }
                else
                {
                    PhysicsMono = null;
                }
            }
            #endregion

            #region cdi3.json
            var isCdiParsed = false;
            if (ParametersCdi.Count > 0 || PartsCdi.Count > 0)
            {
                var cdiJson = new CubismCdi3Json
                {
                    Version = 3,
                    ParameterGroups = Array.Empty<CubismCdi3Json.ParamGroupArray>()
                };

                var parameters = new SortedSet<CubismCdi3Json.ParamGroupArray>();
                foreach (var paramMono in ParametersCdi)
                {
                    var displayName = GetDisplayName(paramMono, assemblyLoader);
                    if (displayName == null)
                        break;

                    paramMono.m_GameObject.TryGet(out var paramGameObject);
                    var paramId = paramGameObject.m_Name;
                    parameters.Add(new CubismCdi3Json.ParamGroupArray
                    {
                        Id = paramId,
                        GroupId = "",
                        Name = displayName
                    });
                }
                cdiJson.Parameters = parameters.ToArray();

                var parts = new SortedSet<CubismCdi3Json.PartArray>();
                foreach (var partMono in PartsCdi)
                {
                    var displayName = GetDisplayName(partMono, assemblyLoader);
                    if (displayName == null)
                        break;

                    partMono.m_GameObject.TryGet(out var partGameObject);
                    var paramId = partGameObject.m_Name;
                    parts.Add(new CubismCdi3Json.PartArray
                    {
                        Id = paramId,
                        Name = displayName
                    });
                }
                cdiJson.Parts = parts.ToArray();

                if (parts.Count > 0 || parameters.Count > 0)
                {
                    File.WriteAllText($"{destPath}{modelName}.cdi3.json", JsonConvert.SerializeObject(cdiJson, Formatting.Indented));
                    isCdiParsed = true;
                }
            }
            #endregion

            #region motion3.json
            var motions = new SortedDictionary<string, JArray>();
            var destMotionPath = Path.Combine(destPath, "motions") + Path.DirectorySeparatorChar;

            if (motionMode == Live2DMotionMode.MonoBehaviour && FadeMotionLst != null) //Fade motions from Fade Motion List
            {
                Logger.Debug("Motion export method: MonoBehaviour (Fade motion)");
                var fadeMotionLstDict = ParseMonoBehaviour(FadeMotionLst, CubismMonoBehaviourType.FadeMotionList, assemblyLoader);
                if (fadeMotionLstDict != null)
                {
                    CubismObjectList.AssetsFile = FadeMotionLst.assetsFile;
                    var fadeMotionAssetList = JsonConvert.DeserializeObject<CubismObjectList>(JsonConvert.SerializeObject(fadeMotionLstDict)).GetFadeMotionAssetList();
                    if (fadeMotionAssetList?.Count > 0)
                    {
                        FadeMotions = fadeMotionAssetList;
                        Logger.Debug($"\"{FadeMotionLst.m_Name}\": found {fadeMotionAssetList.Count} motion(s)");
                    }
                }
            }
            
            if (motionMode == Live2DMotionMode.MonoBehaviour && FadeMotions.Count > 0)  //motion from MonoBehaviour
            {
                ExportFadeMotions(destMotionPath, assemblyLoader, forceBezier, motions);
            }

            if (motions.Count == 0) //motion from AnimationClip
            {
                CubismMotion3Converter converter = null;
                var exportMethod = "AnimationClip";
                if (motionMode != Live2DMotionMode.AnimationClipV1) //AnimationClipV2
                {
                    exportMethod += "V2";
                    converter = new CubismMotion3Converter(AnimationClips, PartNames, ParameterNames);
                }
                else if (GameObjects.Count > 0) //AnimationClipV1
                {
                    exportMethod += "V1";
                    var rootTransform = GameObjects[0].m_Transform;
                    while (rootTransform.m_Father.TryGet(out var m_Father))
                    {
                        rootTransform = m_Father;
                    }
                    rootTransform.m_GameObject.TryGet(out var rootGameObject);
                    converter = new CubismMotion3Converter(rootGameObject, AnimationClips);
                }

                if (motionMode == Live2DMotionMode.MonoBehaviour)
                {
                    exportMethod = FadeMotions.Count > 0
                        ? exportMethod + " (unable to export motions using Fade motion method)"
                        : exportMethod + " (no Fade motions found)";
                }
                Logger.Debug($"Motion export method: {exportMethod}");

                ExportClipMotions(destMotionPath, converter, forceBezier, motions);
            }

            if (motions.Count == 0)
            {
                Logger.Warning($"No exportable motions found for \"{modelName}\" model");
            }
            else
            {
                Logger.Info($"Exported {motions.Count} motion(s)");
            }
            #endregion

            #region exp3.json
            var expressions = new JArray();
            var destExpressionPath = Path.Combine(destPath, "expressions") + Path.DirectorySeparatorChar;

            if (Expressions.Count > 0)
            {
                Directory.CreateDirectory(destExpressionPath);
            }
            foreach (var monoBehaviour in Expressions)
            {
                var expressionName = monoBehaviour.m_Name.Replace(".exp3", "");
                var expressionDict = ParseMonoBehaviour(monoBehaviour, CubismMonoBehaviourType.Expression, assemblyLoader);
                if (expressionDict == null)
                    continue;
                
                var expression = JsonConvert.DeserializeObject<CubismExpression3Json>(JsonConvert.SerializeObject(expressionDict));

                expressions.Add(new JObject
                {
                    { "Name", expressionName },
                    { "File", $"expressions/{expressionName}.exp3.json" }
                });
                File.WriteAllText($"{destExpressionPath}{expressionName}.exp3.json", JsonConvert.SerializeObject(expression, Formatting.Indented));
            }
            #endregion

            #region model3.json
            var groups = new List<CubismModel3Json.SerializableGroup>();

            //Try looking for group IDs among the parameter names manually
            if (EyeBlinkParameters.Count == 0)
            {
                EyeBlinkParameters = ParameterNames.Where(x =>
                    x.ToLower().Contains("eye")
                    && x.ToLower().Contains("open")
                    && (x.ToLower().Contains('l') || x.ToLower().Contains('r'))
                ).ToHashSet();
            }
            if (LipSyncParameters.Count == 0)
            {
                LipSyncParameters = ParameterNames.Where(x =>
                    x.ToLower().Contains("mouth")
                    && x.ToLower().Contains("open")
                    && x.ToLower().Contains('y')
                ).ToHashSet();
            }

            groups.Add(new CubismModel3Json.SerializableGroup
            {
                Target = "Parameter",
                Name = "EyeBlink",
                Ids = EyeBlinkParameters.ToArray()
            });
            groups.Add(new CubismModel3Json.SerializableGroup
            {
                Target = "Parameter",
                Name = "LipSync",
                Ids = LipSyncParameters.ToArray()
            });
            
            var model3 = new CubismModel3Json
            {
                Version = 3,
                Name = modelName,
                FileReferences = new CubismModel3Json.SerializableFileReferences
                {
                    Moc = $"{modelName}.moc3",
                    Textures = textures.ToArray(),
                    DisplayInfo = isCdiParsed ? $"{modelName}.cdi3.json" : null,
                    Physics = PhysicsMono != null ? $"{modelName}.physics3.json" : null,
                    Motions = JObject.FromObject(motions),
                    Expressions = expressions,
                },
                Groups = groups.ToArray()
            };
            File.WriteAllText($"{destPath}{modelName}.model3.json", JsonConvert.SerializeObject(model3, Formatting.Indented));
            #endregion
        }

        private void ExportFadeMotions(string destMotionPath, AssemblyLoader assemblyLoader, bool forceBezier, SortedDictionary<string, JArray> motions)
        {
            Directory.CreateDirectory(destMotionPath);
            foreach (var fadeMotionMono in FadeMotions)
            {
                var fadeMotionDict = ParseMonoBehaviour(fadeMotionMono, CubismMonoBehaviourType.FadeMotion, assemblyLoader);
                if (fadeMotionDict == null)
                    continue;
                
                var fadeMotion = JsonConvert.DeserializeObject<CubismFadeMotion>(JsonConvert.SerializeObject(fadeMotionDict));
                if (fadeMotion.ParameterIds.Length == 0)
                    continue;

                var motionJson = new CubismMotion3Json(fadeMotion, ParameterNames, PartNames, forceBezier);

                var animName = Path.GetFileNameWithoutExtension(fadeMotion.m_Name);
                if (motions.ContainsKey(animName))
                {
                    animName = $"{animName}_{fadeMotion.GetHashCode()}";
                    if (motions.ContainsKey(animName))
                        continue;
                }
                var motionPath = new JObject(new JProperty("File", $"motions/{animName}.motion3.json"));
                motions.Add(animName, new JArray(motionPath));
                File.WriteAllText($"{destMotionPath}{animName}.motion3.json", JsonConvert.SerializeObject(motionJson, Formatting.Indented, new MyJsonConverter()));
            }
        }

        private static void ExportClipMotions(string destMotionPath, CubismMotion3Converter converter, bool forceBezier, SortedDictionary<string, JArray> motions)
        {
            if (converter == null)
                return;

            if (converter.AnimationList.Count > 0)
            {
                Directory.CreateDirectory(destMotionPath);
            }
            foreach (var animation in converter.AnimationList)
            {
                var animName = animation.Name;
                if (animation.TrackList.Count == 0)
                {
                    Logger.Warning($"Motion \"{animName}\" is empty. Export skipped");
                    continue;
                }
                var motionJson = new CubismMotion3Json(animation, forceBezier);
                
                if (motions.ContainsKey(animName))
                {
                    animName = $"{animName}_{animation.GetHashCode()}";

                    if (motions.ContainsKey(animName))
                        continue;
                }
                var motionPath = new JObject(new JProperty("File", $"motions/{animName}.motion3.json"));
                motions.Add(animName, new JArray(motionPath));
                File.WriteAllText($"{destMotionPath}{animName}.motion3.json", JsonConvert.SerializeObject(motionJson, Formatting.Indented, new MyJsonConverter()));
            }
        }

        private static string GetDisplayName(MonoBehaviour cdiMono, AssemblyLoader assemblyLoader)
        {
            var dict = ParseMonoBehaviour(cdiMono, CubismMonoBehaviourType.DisplayInfo, assemblyLoader);
            if (dict == null)
                return null;

            var name = (string)dict["Name"];
            if (dict.Contains("DisplayName"))
            {
                var displayName = (string)dict["DisplayName"];
                name = displayName != "" ? displayName : name;
            }
            return name;
        }
    }
}
