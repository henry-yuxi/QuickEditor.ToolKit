namespace QuickEditor.ToolKit
{
    using UnityEditor;
    using UnityEngine;

    [InitializeOnLoad]
    internal class AssetImportStyles
    {
        public static AssetImportStyles.AudioStyles Audio { private set; get; }
        public static AssetImportStyles.ModelStyles Model { private set; get; }
        public static AssetImportStyles.TextureStyles Texture { private set; get; }

        static AssetImportStyles()
        {
            if (Audio == null) { Audio = new AudioStyles(); }
            if (Model == null) { Model = new ModelStyles(); }
            if (Texture == null) { Texture = new TextureStyles(); }
        }

        internal class AudioStyles
        {
            public GUIContent ForceToMono = new GUIContent("Force To Mono");
            public GUIContent Normalize = new GUIContent("Normalize");
            public GUIContent LoadBackground = new GUIContent("Load In Background");
            public GUIContent Ambisonic = new GUIContent("Ambisonic");

            public GUIContent LoadType = new GUIContent("Load Type");
            public GUIContent PreloadAudioData = new GUIContent("Preload Audio Data");

            public GUIContent CompressionFormat = new GUIContent("Compression Format");
            public GUIContent Quality = new GUIContent("Quality");

            public GUIContent[] CompressionFormatOpts = new GUIContent[]
            {
                new GUIContent("PCM"),
                new GUIContent("Vorbis"),
                new GUIContent("ADPCM"),
            };

            public AudioCompressionFormat[] CompressionEnumOpts = new[]
            {
                AudioCompressionFormat.PCM,
                AudioCompressionFormat.Vorbis,
                AudioCompressionFormat.ADPCM,
            };

            public GUIContent SampleRateSetting = new GUIContent("Sample Rate Setting");
            public GUIContent SampleRate = new GUIContent("Sample Rate");
        }

        internal class ModelStyles
        {
            public GUIContent Meshes = new GUIContent("Meshes", "These options control how geometry is imported.");

            public GUIContent ScaleFactor = new GUIContent("Scale Factor", "How much to scale the models compared to what is in the source file.");

            public GUIContent UseFileUnits = new GUIContent("Use File Units", "Detect file units and import as 1FileUnit=1UnityUnit, otherwise it will import as 1cm=1UnityUnit. See ModelImporter.useFileUnits for more details.");

            public GUIContent FileScaleFactor = new GUIContent("File Scale", "暂时不需要打开"/*"Model scale defined in the source file. If available."*/);

            public GUIContent ImportBlendShapes = new GUIContent("Import BlendShapes", "Should Unity import BlendShapes.");

            public GUIContent GenerateColliders = new GUIContent("Generate Colliders", "Should Unity generate mesh colliders for all meshes.");

            public GUIContent SwapUVChannels = new GUIContent("Swap UVs", "Swaps the 2 UV channels in meshes. Use if your diffuse texture uses UVs from the lightmap.");

            public GUIContent GenerateSecondaryUV = new GUIContent("Generate Lightmap UVs", "Generate lightmap UVs into UV2.");

            public GUIContent GenerateSecondaryUVAdvanced = new GUIContent("Advanced");

            public GUIContent GenerateMeshes = new GUIContent("Meshes", "Fold Mesh Settings!");

            public GUIContent secondaryUVAngleDistortion = new GUIContent("Angle Error", "Measured in percents. Angle error measures deviation of UV angles from geometry angles. Area error measures deviation of UV triangles area from geometry triangles if they were uniformly scaled.");

            public GUIContent secondaryUVAreaDistortion = new GUIContent("Area Error");

            public GUIContent secondaryUVHardAngle = new GUIContent("Hard Angle", "Angle between neighbor triangles that will generate seam.");

            public GUIContent secondaryUVPackMargin = new GUIContent("Pack Margin", "Measured in pixels, assuming mesh will cover an entire 1024x1024 lightmap.");

            public GUIContent secondaryUVDefaults = new GUIContent("Set Defaults");

            public GUIContent TangentSpace = new GUIContent("Normals & Tangents");

            public GUIContent TangentSpaceNormalLabel = new GUIContent("Normals");

            public GUIContent TangentSpaceTangentLabel = new GUIContent("Tangents");

            public GUIContent TangentSpaceOptionImport = new GUIContent("Import");

            public GUIContent TangentSpaceOptionCalculateLegacy = new GUIContent("Calculate Legacy");

            public GUIContent TangentSpaceOptionCalculateLegacySplit = new GUIContent("Calculate Legacy - Split Tangents");

            public GUIContent TangentSpaceOptionCalculate = new GUIContent("Calculate Tangent Space");

            public GUIContent TangentSpaceOptionNone = new GUIContent("None");

            public GUIContent TangentSpaceOptionNoneNoNormals = new GUIContent("None - (Normals required)");

            public GUIContent NormalOptionImport = new GUIContent("Import");

            public GUIContent NormalOptionCalculate = new GUIContent("Calculate");

            public GUIContent NormalOptionNone = new GUIContent("None");

            public GUIContent[] TestNormalOption = new GUIContent[]
            {
                new GUIContent("Import"),
                new GUIContent("Calculate"),
                new GUIContent("None")
            };

            public GUIContent[] TangentSpaceModeOptLabelsAll;

            public GUIContent[] TangentSpaceModeOptLabelsCalculate;

            public GUIContent[] TangentSpaceModeOptLabelsNone;

            public GUIContent[] NormalModeLabelsAll;

            public ModelImporterTangents[] TangentSpaceModeOptEnumsAll;

            public ModelImporterTangents[] TangentSpaceModeOptEnumsCalculate;

            public ModelImporterTangents[] TangentSpaceModeOptEnumsNone;

            public GUIContent SmoothingAngle = new GUIContent("Smoothing Angle", "Normal Smoothing Angle");

            public GUIContent MeshCompressionLabel = new GUIContent("Mesh Compression", "Higher compression ratio means lower mesh precision. If enabled, the mesh bounds and a lower bit depth per component are used to compress the mesh data.");

            public GUIContent[] MeshCompressionOpt = new GUIContent[]
                {
                    new GUIContent("Off"),
                    new GUIContent("Low"),
                    new GUIContent("Medium"),
                    new GUIContent("High")
                };

            public GUIContent OptimizeMeshForGPU = new GUIContent("Optimize Mesh", "The vertices and indices will be reordered for better GPU performance.");

            public GUIContent KeepQuads = new GUIContent("Keep Quads", "If model contains quad faces, they are kept for DX11 tessellation.");

            public GUIContent WeldVertices = new GUIContent("Weld Vertices", "Combine vertices that share the same position in space.");

            public GUIContent IsReadable = new GUIContent("Read/Write Enabled", "Allow vertices and indices to be accessed from script.");

            public GUIContent Materials = new GUIContent("Materials");

            public GUIContent ImportMaterials = new GUIContent("Import Materials");

            public GUIContent MaterialName = new GUIContent("Material Naming");

            public GUIContent MaterialNameTex = new GUIContent("By Base Texture Name");

            public GUIContent MaterialNameMat = new GUIContent("From Model's Material");

            public GUIContent[] MaterialNameOptMain = new GUIContent[]
                {
                    new GUIContent("By Base Texture Name"),
                    new GUIContent("From Model's Material"),
                    new GUIContent("Model Name + Model's Material")
                };

            public GUIContent MaterialSearch = new GUIContent("Material Search");

            public GUIContent[] MaterialSearchOpt = new GUIContent[]
                {
                    new GUIContent("Local Materials Folder"),
                    new GUIContent("Recursive-Up"),
                    new GUIContent("Project-Wide")
                };

            public GUIContent MaterialHelpStart = new GUIContent("For each imported material, Unity first looks for an existing material named %MAT%.");

            public GUIContent MaterialHelpEnd = new GUIContent("If it doesn't exist, a new one is created in the local Materials folder.");

            public GUIContent MaterialHelpDefault = new GUIContent("No new materials are generated. Unity's Default-Diffuse material is used instead.");

            public GUIContent[] MaterialNameHelp = new GUIContent[]
                {
                    new GUIContent("[BaseTextureName]"),
                    new GUIContent("[MaterialName]"),
                    new GUIContent("[ModelFileName]-[MaterialName]"),
                    new GUIContent("[BaseTextureName] or [ModelFileName]-[MaterialName] if no base texture can be found")
                };

            public GUIContent[] MaterialSearchHelp = new GUIContent[]
                {
                    new GUIContent("Unity will look for it in the local Materials folder."),
                    new GUIContent("Unity will do a recursive-up search for it in all Materials folders up to the Assets folder."),
                    new GUIContent("Unity will search for it anywhere inside the Assets folder.")
                };

            public GUIContent GenerateRigs = new GUIContent("Rigs", "Fold Out Rigs!");

            public ModelStyles()
            {
                this.NormalModeLabelsAll = new GUIContent[]
                    {
                        this.NormalOptionImport,
                        this.NormalOptionCalculate,
                        this.NormalOptionNone
                    };
                this.TangentSpaceModeOptLabelsAll = new GUIContent[]
                    {
                        this.TangentSpaceOptionImport,
                        this.TangentSpaceOptionCalculate,
                        this.TangentSpaceOptionCalculateLegacy,
                        this.TangentSpaceOptionCalculateLegacySplit,
                        this.TangentSpaceOptionNone
                    };
                this.TangentSpaceModeOptLabelsCalculate = new GUIContent[]
                    {
                        this.TangentSpaceOptionCalculate,
                        this.TangentSpaceOptionCalculateLegacy,
                        this.TangentSpaceOptionCalculateLegacySplit,
                        this.TangentSpaceOptionNone
                    };
                this.TangentSpaceModeOptLabelsNone = new GUIContent[]
                    {
                        this.TangentSpaceOptionNoneNoNormals
                    };
                this.TangentSpaceModeOptEnumsAll = new ModelImporterTangents[]
                    {
                        ModelImporterTangents.Import,
                        ModelImporterTangents.CalculateMikk,
                        ModelImporterTangents.CalculateLegacy,
                        ModelImporterTangents.CalculateLegacyWithSplitTangents,
                        ModelImporterTangents.None
                    };
                this.TangentSpaceModeOptEnumsCalculate = new ModelImporterTangents[]
                    {
                        ModelImporterTangents.CalculateMikk,
                        ModelImporterTangents.CalculateLegacy,
                        ModelImporterTangents.CalculateLegacyWithSplitTangents,
                        ModelImporterTangents.None
                    };
                this.TangentSpaceModeOptEnumsNone = new ModelImporterTangents[]
                    {
                        ModelImporterTangents.None
                    };
            }

            public GUIContent AnimationType = new GUIContent("Animation Type", "The type of animation to support / import.");

            public GUIContent[] AnimationTypeOpt = new GUIContent[]
                {
                    new GUIContent("None","No animation present."),
                    new GUIContent("Legacy","Legacy animation system."),
                    new GUIContent("Generic","Generic Mecanim animation."),
                    new GUIContent("Humanoid","Humanoid Mecanim animation system.")
                };

            public GUIContent AnimLabel = new GUIContent("Generation|Controls how animations are imported.");

            public GUIContent[] AnimationsOpt = new GUIContent[]
                {
                    new GUIContent("Don't Import","No animation or skinning is imported."),
                    new GUIContent("Store in Original Roots (Deprecated)","Animations are stored in the root objects of your animation package (these might be different from the root objects in Unity)."),
                    new GUIContent("Store in Nodes (Deprecated)","Animations are stored together with the objects they animate. Use this when you have a complex animation setup and want full scripting control."),
                    new GUIContent("Store in Root (Deprecated)","Animations are stored in the scene's transform root objects. Use this when animating anything that has a hierarchy."),
                    new GUIContent("Store in Root (New)")
                };

            //public GUIStyle helpText = new GUIStyle(EditorStyles.helpBox);

            public GUIContent avatar = new GUIContent("Animator");

            public GUIContent configureAvatar = new GUIContent("Configure...");

            public GUIContent avatarValid = new GUIContent("✓");

            public GUIContent avatarInvalid = new GUIContent("✕");

            public GUIContent avatarPending = new GUIContent("...");

            public GUIContent UpdateMuscleDefinitionFromSource = new GUIContent("Update|Update the copy of the muscle definition from the source.");

            public GUIContent RootNode = new GUIContent("Root node|Specify the root node used to extract the animation translation.");

            public GUIContent AvatarDefinition = new GUIContent("Avatar Definition|Choose between Create From This Model or Copy From Other Avatar. The first one create an Avatar for this file and the second one use an Avatar from another file to import animation.");

            public GUIContent[] AvatarDefinitionOpt = new GUIContent[]
                {
                    new GUIContent("Create From This Model|Create an Avatar based on the model from this file."),
                    new GUIContent("Copy From Other Avatar|Copy an Avatar from another file to import muscle clip. No avatar will be created.")
                };

            public GUIContent UpdateReferenceClips = new GUIContent("Update reference clips|Click on this button to update all the @convention file referencing this file. Should set all these files to Copy From Other Avatar, set the source Avatar to this one and reimport all these files.");

            //public Styles()
            //{
            //    this.helpText.normal.background = null;
            //    this.helpText.alignment = TextAnchor.MiddleLeft;
            //    this.helpText.padding = new RectOffset(0, 0, 0, 0);
            //}
            public GUIContent GenerateAnimation = new GUIContent("Animations");

            public GUIContent ImportAnimation = new GUIContent("Import Animation");

            public GUIContent AnimationCompression = new GUIContent("Anim.Compression");

            public GUIContent[] AnimationCompressionOpts = new GUIContent[]
            {
            new GUIContent("Off"),
            new GUIContent("KeyframeReduction"),
            new GUIContent("KeyframeReductionAndCompression"),
            };

            public ModelImporterAnimationCompression[] AnimationCompressionEnumOpts = new[]
            {
            ModelImporterAnimationCompression.Off,
            ModelImporterAnimationCompression.KeyframeReduction,
            ModelImporterAnimationCompression.KeyframeReductionAndCompression,
        };
        }

        internal class TextureStyles
        {
            public GUIContent TextureType = new GUIContent("Texture Type");

            public GUIContent[] TextureTypeOpts = new GUIContent[]
            {
            new GUIContent("Default"),
            new GUIContent("Lightmap"),
            new GUIContent("NormalMap"),
            new GUIContent("Sprite"),
            new GUIContent("SingleChannel"),
            };

            public TextureImporterType[] TextureTypeEnumOpts = new TextureImporterType[]
            {
            TextureImporterType.Default,
            TextureImporterType.Lightmap,
            TextureImporterType.NormalMap,
            TextureImporterType.Sprite,
            TextureImporterType.SingleChannel
            };

            public GUIContent Advanced = new GUIContent("Advanced");

            public GUIContent NonPowerof2 = new GUIContent("Non Power of 2");

            public GUIContent ReadWriteEnable = new GUIContent("Read/Write Enabled", "Enable to be able to access ");

            public GUIContent GenMipMaps = new GUIContent("Generate Mip Maps");

            public GUIContent AlphaSource = new GUIContent("Alpha Source");

            public GUIContent WrapMode = new GUIContent("Wrap Mode");

            public GUIContent FilterMode = new GUIContent("Filter Mode");

            public GUIContent AnisoLevel = new GUIContent("Aniso Level");

            public GUIContent MaxSize = new GUIContent("Max Size");

            public GUIContent[] MaxSizeOpts = new GUIContent[]
            {
                new GUIContent("128"),new GUIContent("256"),
                new GUIContent("512"),new GUIContent("1024"),
                new GUIContent("2048"),new GUIContent("4096"),
            };

            public GUIContent Compress = new GUIContent("Compression");

            public GUIContent CompressFormat = new GUIContent("Compression Format");

            public GUIContent CompressionQuality = new GUIContent("Compression Quality");

            public TextureStyles()
            {
            }
        }
    }
}