namespace QuickEditor.ToolKit
{
    using UnityEditor;

    public class ModelImportRuleGUI : AssetImportRuleGUI<ModelImportRule>
    {
        public override void OnInit()
        {
            base.OnInit();
        }

        protected override void DrawLeftGUI()
        {
            base.DrawLeftGUI();
            DoReorderableList(AssetImportRuleSettings.Current.ModelImportRuleSettings, typeof(ModelImportRule));
        }

        protected override void DrawRightGUI()
        {
            base.DrawRightGUI();
            using (var scroll = new EditorGUILayout.ScrollViewScope(mAssetRuleScroll))
            {
                mAssetRuleScroll = scroll.scrollPosition;
                if (mAssetImportRuleList.index >= AssetImportRuleSettings.Current.ModelImportRuleSettings.Count)
                {
                    mAssetImportRuleList.index = 0;
                }
                if (AssetImportRuleSettings.Current.ModelImportRuleSettings.Count > 0)
                {
                    AssetImportRuleSettings.Current.ModelImportRuleSettings[mAssetImportRuleList.index].DrawRuleGUI();
                }
            }
        }

        /// <summary>
        /// inspector panel
        /// </summary>
        //        public override void OnRuleGUI(ModelImportRule assetRule)
        //        {
        //            base.OnRuleGUI(assetRule);

        //            GUILayout.BeginHorizontal();
        //            GUILayout.FlexibleSpace();
        //            if (GUILayout.Toggle(this.m_pageIndex == 0, "Model", "ButtonLeft" , GUILayout.MaxWidth(150))) this.m_pageIndex = 0;
        //            if (GUILayout.Toggle(this.m_pageIndex == 1, "Rig", "ButtonMid", GUILayout.MaxWidth(100))) this.m_pageIndex = 1;
        //            if (GUILayout.Toggle(this.m_pageIndex == 2, "Animations", "ButtonRight", GUILayout.MaxWidth(150))) this.m_pageIndex = 2;
        //            GUILayout.FlexibleSpace();
        //            GUILayout.EndHorizontal();

        //            GUILayout.Space(5);

        //            //            m_MeshFold = EditorGUILayout.Foldout(this.m_MeshFold,
        //            //                    styles.GenerateMeshes, EditorStyles.foldout);

        //            if (m_pageIndex == 0) //mesh
        //            {
        //                ModelPageGUI(assetRule);
        //                CommomUI(assetRule);
        //                NormalsAndTangentsGUI(assetRule);
        //                MaterialsGUI(assetRule);
        //            }

        ////            m_RigFold = EditorGUILayout.Foldout(this.m_RigFold,
        ////                    styles.GenerateRigs, EditorStyles.foldout);

        //            if (m_pageIndex == 1) //rig
        //            {
        //                assetRule.AnimationType = (ModelImporterAnimationType)
        //                    EditorGUILayout.EnumPopup(styles.AnimationType, assetRule.AnimationType);

        //                if (assetRule.AnimationType == ModelImporterAnimationType.Generic ||
        //                    assetRule.AnimationType == ModelImporterAnimationType.Human)
        //                {
        //                    assetRule.isOptimizeObject = true;
        //                }
        //                else
        //                {
        //                    assetRule.isOptimizeObject = false;
        //                }
        //            }

        ////            m_AnimationsFold = EditorGUILayout.Foldout(this.m_AnimationsFold,
        ////                   styles.GenerateAnimation, EditorStyles.foldout);

        //            if (m_pageIndex == 2)  //animation
        //            {
        //                assetRule.ImportAnimation = EditorGUILayout.Toggle(styles.ImportAnimation, assetRule.ImportAnimation);

        //                if (assetRule.ImportAnimation)
        //                {
        //                    int acIndex = Array.FindIndex(styles.AnimationCompressionEnumOpts,compress => compress == assetRule.AnimCompression);
        //                    acIndex = Math.Max(acIndex, 0);
        //                    acIndex = EditorGUILayout.Popup(styles.AnimationCompression, acIndex, styles.AnimationCompressionOpts);
        //                    assetRule.AnimCompression = styles.AnimationCompressionEnumOpts[acIndex];
        //                }
        //            }

        //            //CommomUI(assetRule);
        //            //NormalsAndTangentsGUI(assetRule);
        //            //MaterialsGUI(assetRule);

        //        }

        private void ModelPageGUI(ModelImportRule assetRule)
        {
        }

        private void CommomUI(ModelImportRule assetRule)
        {
            //using (new EditorGUI.DisabledScope(base.targets.Length > 1))
            //{
            //    assetRule.m_GlobalScale = EditorGUILayout.FloatField(styles.ScaleFactor, assetRule.m_GlobalScale,
            //        new GUILayoutOption[0]);
            //}

            //using (new EditorGUI.DisabledScope(true))
            //{
            //    assetRule.m_FileScale = EditorGUILayout.FloatField(styles.FileScaleFactor, assetRule.m_FileScale);
            //}

            //assetRule.m_MeshCompression =
            //    (ModelImporterMeshCompression)
            //        EditorGUILayout.EnumPopup(styles.MeshCompressionLabel, assetRule.m_MeshCompression);
            //assetRule.m_IsReadable = EditorGUILayout.Toggle(styles.IsReadable, assetRule.m_IsReadable);
            //assetRule.optimizeMeshForGPU = EditorGUILayout.Toggle(styles.OptimizeMeshForGPU, assetRule.optimizeMeshForGPU);
            //assetRule.m_ImportBlendShapes = EditorGUILayout.Toggle(styles.ImportBlendShapes, assetRule.m_ImportBlendShapes);
            //assetRule.m_AddColliders = EditorGUILayout.Toggle(styles.GenerateColliders, assetRule.m_AddColliders);

            //using (new EditorGUI.DisabledScope(true))
            //{
            //    assetRule.keepQuads = EditorGUILayout.Toggle(styles.KeepQuads, assetRule.keepQuads);
            //}
            //assetRule.m_weldVertices = EditorGUILayout.Toggle(styles.WeldVertices, assetRule.m_weldVertices);
            //assetRule.swapUVChannels = EditorGUILayout.Toggle(styles.SwapUVChannels, assetRule.swapUVChannels);
            //assetRule.generateSecondaryUV = EditorGUILayout.Toggle(styles.GenerateSecondaryUV, assetRule.generateSecondaryUV);
            //if (assetRule.generateSecondaryUV)
            //{
            //    //EditorGUI.indentLevel++;
            //    //this.m_SecondaryUVAdvancedOptions = EditorGUILayout.Foldout(this.m_SecondaryUVAdvancedOptions,
            //    //    styles.GenerateSecondaryUVAdvanced, EditorStyles.foldout);
            //    //if (this.m_SecondaryUVAdvancedOptions)
            //    //{
            //    //    assetRule.secondaryUVHardAngle = EditorGUILayout.Slider(styles.secondaryUVHardAngle,
            //    //        assetRule.secondaryUVHardAngle, 0f, 180f, new GUILayoutOption[0]);
            //    //    assetRule.secondaryUVPackMargin = EditorGUILayout.Slider(styles.secondaryUVPackMargin,
            //    //        assetRule.secondaryUVPackMargin, 0f, 180f, new GUILayoutOption[0]);
            //    //    assetRule.secondaryUVAngleDistortion = EditorGUILayout.Slider(styles.secondaryUVAngleDistortion,
            //    //        assetRule.secondaryUVAngleDistortion, 0f, 180f, new GUILayoutOption[0]);
            //    //    assetRule.secondaryUVAreaDistortion = EditorGUILayout.Slider(styles.secondaryUVAreaDistortion,
            //    //        assetRule.secondaryUVAreaDistortion, 0f, 180f, new GUILayoutOption[0]);
            //    //}
            //    //EditorGUI.indentLevel--;
            //}
        }

        private void NormalsAndTangentsGUI(ModelImportRule rule)
        {
            //GUILayout.Label(styles.TangentSpace, EditorStyles.boldLabel, new GUILayoutOption[0]);

            //EditorGUI.BeginChangeCheck();
            //rule.normalImportMode = (ModelImporterNormals)EditorGUILayout.EnumPopup(styles.TangentSpaceNormalLabel, rule.normalImportMode);
            //if (EditorGUI.EndChangeCheck())
            //{
            //    if (rule.normalImportMode == ModelImporterNormals.None)
            //    {
            //        rule.tangentImportMode = ModelImporterTangents.None;
            //    }
            //    //                else if (rule.normalImportMode == ModelImporterNormals.Import)
            //    //                {
            //    //                    rule.tangentImportMode = ModelImporterTangents.Import;
            //    //                }
            //    else
            //    {
            //        rule.tangentImportMode = ModelImporterTangents.CalculateMikk;
            //    }
            //}

            //using (new EditorGUI.DisabledScope(rule.normalImportMode != ModelImporterNormals.Calculate))
            //{
            //    rule.normalSmoothAngle = EditorGUILayout.Slider(styles.SmoothingAngle, rule.normalSmoothAngle, 0f, 180f);
            //}
            //GUIContent[] displayedOptions = styles.TangentSpaceModeOptLabelsAll;
            //ModelImporterTangents[] array = styles.TangentSpaceModeOptEnumsAll;
            //if (rule.normalImportMode == ModelImporterNormals.Calculate)
            //{
            //    displayedOptions = styles.TangentSpaceModeOptLabelsCalculate;
            //    array = styles.TangentSpaceModeOptEnumsCalculate;
            //}
            //else if (rule.normalImportMode == ModelImporterNormals.None)
            //{
            //    displayedOptions = styles.TangentSpaceModeOptLabelsNone;
            //    array = styles.TangentSpaceModeOptEnumsNone;
            //}
            //using (new EditorGUI.DisabledScope(rule.normalImportMode == ModelImporterNormals.None))
            //{
            //    int num = Array.IndexOf(array, rule.tangentImportMode);
            //    EditorGUI.BeginChangeCheck();
            //    num = EditorGUILayout.Popup(styles.TangentSpaceTangentLabel, num, displayedOptions, new GUILayoutOption[0]);
            //    if (EditorGUI.EndChangeCheck())
            //    {
            //        rule.tangentImportMode = array[num];
            //    }
            //}
        }

        private void MaterialsGUI(ModelImportRule rule)
        {
            //GUILayout.Label(styles.Materials, EditorStyles.boldLabel, new GUILayoutOption[0]);
            //rule.m_ImportMaterials = EditorGUILayout.Toggle(styles.ImportMaterials, rule.m_ImportMaterials);
            //string text;
            //if (rule.m_ImportMaterials)
            //{
            //    rule.m_MaterialName = (ModelImporterMaterialName)EditorGUILayout.Popup(styles.MaterialName, (int)rule.m_MaterialName, styles.MaterialNameOptMain);
            //    rule.m_MaterialSearch = (ModelImporterMaterialSearch)EditorGUILayout.Popup(styles.MaterialSearch, (int)rule.m_MaterialSearch, styles.MaterialSearchOpt);
            //    text = string.Concat(styles.MaterialHelpStart.text.Replace("%MAT%", styles.MaterialNameHelp[(int)rule.m_MaterialName].text), "\n", styles.MaterialSearchHelp[(int)rule.m_MaterialSearch].text, "\n", styles.MaterialHelpEnd.text);
            //}
            //else
            //{
            //    text = styles.MaterialHelpDefault.text;
            //}
            //GUILayout.Label(new GUIContent(text), EditorStyles.helpBox, new GUILayoutOption[0]);
        }
    }
}