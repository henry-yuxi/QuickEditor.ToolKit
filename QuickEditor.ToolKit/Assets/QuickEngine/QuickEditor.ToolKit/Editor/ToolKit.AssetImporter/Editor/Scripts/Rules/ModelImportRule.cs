namespace QuickEditor.ToolKit
{
    using QuickEditor.Common;
    using System;
    using UnityEditor;
    using UnityEngine;

    [System.Serializable]
    public class ModelImportRule : AssetImportRule
    {
        //Model Settings
        public float scaleFactor = 1;

        public bool importBlendShapes;

        public ModelImporterMeshCompression meshCompression;
        public bool isReadable;
        public bool optimizeMeshForGPU;
        public bool generateColliders;

        public bool keepQuads;
        public bool weldVertices;
        public bool swapUVChannels;
        public bool generateSecondaryUV;

        public float secondaryUVHardAngle;
        public float secondaryUVPackMargin;
        public float secondaryUVAngleDistortion;
        public float secondaryUVAreaDistortion;
        public float normalSmoothAngle;
        public ModelImporterNormals normalImportMode;

        public ModelImporterTangents tangentImportMode;
        public ModelImporterMaterialName materialName;
        public ModelImporterMaterialSearch materialSearch;

        //Rig Settings
        public ModelImporterAnimationType animationType;

        public bool isOptimizeObject;

        //Animation Settings
        public bool importAnimation;

        public ModelImporterAnimationCompression animCompression;

        //Materials Settings
        public bool importMaterials;

        public override bool IsMatch(UnityEditor.AssetImporter importer)
        {
            if (importer is ModelImporter)
            {
                return true;
            }
            return false;
        }

        public override void ApplyDefaults()
        {
            this.RuleName = "New Model Rule";
            SuffixFilter = ".FBX";

            meshCompression = ModelImporterMeshCompression.Off;
            isReadable = false;
            importBlendShapes = false;
            optimizeMeshForGPU = true;
            generateColliders = false;
            keepQuads = false;
            weldVertices = true;
            swapUVChannels = false;
            generateSecondaryUV = false;
            normalImportMode = ModelImporterNormals.None;
            tangentImportMode = ModelImporterTangents.CalculateMikk;
            importMaterials = true;
            materialName = ModelImporterMaterialName.BasedOnTextureName;
            materialSearch = ModelImporterMaterialSearch.Everywhere;

            animationType = ModelImporterAnimationType.None;

            importAnimation = false;
            animCompression = ModelImporterAnimationCompression.Off;
        }

        public override void ApplySettings(UnityEditor.AssetImporter assetImporter)
        {
            ModelImporter importer = assetImporter as ModelImporter;
            if (importer == null) { return; }
            importer.globalScale = scaleFactor;
            importer.meshCompression = meshCompression;
            importer.isReadable = isReadable;
            importer.optimizeMesh = optimizeMeshForGPU;
            importer.importBlendShapes = importBlendShapes;
            importer.addCollider = generateColliders;
            importer.keepQuads = keepQuads;
            importer.weldVertices = weldVertices;
            importer.swapUVChannels = swapUVChannels;
            importer.generateSecondaryUV = generateSecondaryUV;

            //Normals & Tangents
            importer.importNormals = normalImportMode;
            importer.normalSmoothingAngle = normalSmoothAngle;
            importer.importTangents = tangentImportMode;
            importer.importMaterials = importMaterials;
            importer.materialName = materialName;

            //rig
            importer.animationType = animationType;
            importer.optimizeGameObjects = isOptimizeObject;

            //animations
            importer.importAnimation = importAnimation;
            importer.animationCompression = animCompression;

            Debug.Log("Modifying Model Import Settings, An Import will now occur and the settings will be checked to be OK again during that import");
            //importer.SaveAndReimport();
        }

        private int m_pageIndex;
        private string[] mPlatformNames = new string[] { "Model", "Rig", "Animations", "Materials" };

        public override void DrawRuleGUI()
        {
            base.DrawRuleGUI();
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Model Import Settings", EditorStyles.boldLabel);
            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 2), QuickEditorColors.DarkGrayX11);
            GUILayout.Space(3);

            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            QuickGUILayout.Toolbar(ref m_pageIndex, mPlatformNames, EditorStyles.toolbarButton, GUILayout.ExpandWidth(true));
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            switch (m_pageIndex)
            {
                case 0: DrawModelGUI(); break;
                case 1: DrawRigGUI(); break;
                case 2: DrawAnimationsGUI(); break;
                case 3: DrawMaterialsGUI(); break;
                default:
                    break;
            }
        }

        private void DrawModelGUI()
        {
            EditorGUILayout.LabelField("Scene", EditorStyles.boldLabel);
            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 2), QuickEditorColors.DarkGrayX11);
            GUILayout.Space(3);

            scaleFactor = EditorGUILayout.FloatField("Scale Factor", scaleFactor);
            importBlendShapes = EditorGUILayout.Toggle(AssetImportStyles.Model.ImportBlendShapes, importBlendShapes);

            GUILayout.Space(3);
            EditorGUILayout.LabelField("Meshes", EditorStyles.boldLabel);
            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 2), QuickEditorColors.DarkGrayX11);
            GUILayout.Space(3);

            meshCompression = (ModelImporterMeshCompression)EditorGUILayout.EnumPopup(AssetImportStyles.Model.MeshCompressionLabel, meshCompression);
            isReadable = EditorGUILayout.Toggle(AssetImportStyles.Model.IsReadable, isReadable);
            optimizeMeshForGPU = EditorGUILayout.Toggle(AssetImportStyles.Model.OptimizeMeshForGPU, optimizeMeshForGPU);

            generateColliders = EditorGUILayout.Toggle(AssetImportStyles.Model.GenerateColliders, generateColliders);
            GUILayout.Space(5);
            EditorGUILayout.LabelField("Geometry", EditorStyles.boldLabel);
            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 2), QuickEditorColors.DarkGrayX11);
            GUILayout.Space(3);
            using (new EditorGUI.DisabledScope(true))
            {
                keepQuads = EditorGUILayout.Toggle(AssetImportStyles.Model.KeepQuads, keepQuads);
            }
            weldVertices = EditorGUILayout.Toggle(AssetImportStyles.Model.WeldVertices, weldVertices);

            GUILayout.Label(AssetImportStyles.Model.TangentSpace, EditorStyles.boldLabel, new GUILayoutOption[0]);

            EditorGUI.BeginChangeCheck();
            normalImportMode = (ModelImporterNormals)EditorGUILayout.EnumPopup(AssetImportStyles.Model.TangentSpaceNormalLabel, normalImportMode);
            if (EditorGUI.EndChangeCheck())
            {
                if (normalImportMode == ModelImporterNormals.None)
                {
                    tangentImportMode = ModelImporterTangents.None;
                }
                //                else if (rule.normalImportMode == ModelImporterNormals.Import)
                //                {
                //                    rule.tangentImportMode = ModelImporterTangents.Import;
                //                }
                else
                {
                    tangentImportMode = ModelImporterTangents.CalculateMikk;
                }
            }

            using (new EditorGUI.DisabledScope(normalImportMode != ModelImporterNormals.Calculate))
            {
                normalSmoothAngle = EditorGUILayout.Slider(AssetImportStyles.Model.SmoothingAngle, normalSmoothAngle, 0f, 180f);
            }
            GUIContent[] displayedOptions = AssetImportStyles.Model.TangentSpaceModeOptLabelsAll;
            ModelImporterTangents[] array = AssetImportStyles.Model.TangentSpaceModeOptEnumsAll;
            if (normalImportMode == ModelImporterNormals.Calculate)
            {
                displayedOptions = AssetImportStyles.Model.TangentSpaceModeOptLabelsCalculate;
                array = AssetImportStyles.Model.TangentSpaceModeOptEnumsCalculate;
            }
            else if (normalImportMode == ModelImporterNormals.None)
            {
                displayedOptions = AssetImportStyles.Model.TangentSpaceModeOptLabelsNone;
                array = AssetImportStyles.Model.TangentSpaceModeOptEnumsNone;
            }
            using (new EditorGUI.DisabledScope(normalImportMode == ModelImporterNormals.None))
            {
                int num = Array.IndexOf(array, tangentImportMode);
                EditorGUI.BeginChangeCheck();
                num = EditorGUILayout.Popup(AssetImportStyles.Model.TangentSpaceTangentLabel, num, displayedOptions, new GUILayoutOption[0]);
                if (EditorGUI.EndChangeCheck())
                {
                    tangentImportMode = array[num];
                }
            }

            swapUVChannels = EditorGUILayout.Toggle(AssetImportStyles.Model.SwapUVChannels, swapUVChannels);
            generateSecondaryUV = EditorGUILayout.Toggle(AssetImportStyles.Model.GenerateSecondaryUV, generateSecondaryUV);
            if (generateSecondaryUV)
            {
                //EditorGUI.indentLevel++;
                //this.m_SecondaryUVAdvancedOptions = EditorGUILayout.Foldout(this.m_SecondaryUVAdvancedOptions,
                //    styles.GenerateSecondaryUVAdvanced, EditorStyles.foldout);
                //if (this.m_SecondaryUVAdvancedOptions)
                //{
                //    assetRule.secondaryUVHardAngle = EditorGUILayout.Slider(styles.secondaryUVHardAngle,
                //        assetRule.secondaryUVHardAngle, 0f, 180f, new GUILayoutOption[0]);
                //    assetRule.secondaryUVPackMargin = EditorGUILayout.Slider(styles.secondaryUVPackMargin,
                //        assetRule.secondaryUVPackMargin, 0f, 180f, new GUILayoutOption[0]);
                //    assetRule.secondaryUVAngleDistortion = EditorGUILayout.Slider(styles.secondaryUVAngleDistortion,
                //        assetRule.secondaryUVAngleDistortion, 0f, 180f, new GUILayoutOption[0]);
                //    assetRule.secondaryUVAreaDistortion = EditorGUILayout.Slider(styles.secondaryUVAreaDistortion,
                //        assetRule.secondaryUVAreaDistortion, 0f, 180f, new GUILayoutOption[0]);
                //}
                //EditorGUI.indentLevel--;
            }
        }

        private void DrawRigGUI()
        {
            animationType = (ModelImporterAnimationType)EditorGUILayout.EnumPopup(AssetImportStyles.Model.AnimationType, animationType);

            if (animationType == ModelImporterAnimationType.Generic ||
                animationType == ModelImporterAnimationType.Human)
            {
                isOptimizeObject = true;
            }
            else
            {
                isOptimizeObject = false;
            }
        }

        private void DrawAnimationsGUI()
        {
            importAnimation = EditorGUILayout.Toggle(AssetImportStyles.Model.ImportAnimation, importAnimation);

            if (importAnimation)
            {
                int acIndex = Array.FindIndex(AssetImportStyles.Model.AnimationCompressionEnumOpts, compress => compress == animCompression);
                acIndex = Math.Max(acIndex, 0);
                acIndex = EditorGUILayout.Popup(AssetImportStyles.Model.AnimationCompression, acIndex, AssetImportStyles.Model.AnimationCompressionOpts);
                animCompression = AssetImportStyles.Model.AnimationCompressionEnumOpts[acIndex];
            }
        }

        private void DrawMaterialsGUI()
        {
            importMaterials = EditorGUILayout.Toggle(AssetImportStyles.Model.ImportMaterials, importMaterials);
            string tip;
            if (importMaterials)
            {
                materialName = (ModelImporterMaterialName)EditorGUILayout.Popup(AssetImportStyles.Model.MaterialName, (int)materialName, AssetImportStyles.Model.MaterialNameOptMain);
                materialSearch = (ModelImporterMaterialSearch)EditorGUILayout.Popup(AssetImportStyles.Model.MaterialSearch, (int)materialSearch, AssetImportStyles.Model.MaterialSearchOpt);
                tip = string.Concat(AssetImportStyles.Model.MaterialHelpStart.text.Replace("%MAT%", AssetImportStyles.Model.MaterialNameHelp[(int)materialName].text), "\n", AssetImportStyles.Model.MaterialSearchHelp[(int)materialSearch].text, "\n", AssetImportStyles.Model.MaterialHelpEnd.text);
            }
            else
            {
                tip = AssetImportStyles.Model.MaterialHelpDefault.text;
            }
            GUILayout.Label(new GUIContent(tip), EditorStyles.helpBox, new GUILayoutOption[0]);
        }
    }
}