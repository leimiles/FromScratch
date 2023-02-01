using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;
using System.IO;


namespace MilesRenderingPipeline {
    [ExcludeFromPreset]
    // mrp Asset
    public class MilesRenderingPipelineAsset : RenderPipelineAsset, ISerializationCallbackReceiver {
        ScriptableRenderer[] m_Renderers = new ScriptableRenderer[1];
        [SerializeField] internal int m_DefaultRendererIndex = 0;
        [SerializeField] internal ScriptableRendererData[] m_RendererDataList = new ScriptableRendererData[1];
        // different rendererType
        public enum MilesRendererType {
            MilesRenderer
        }
        public ScriptableRendererData scriptableRendererData {
            get {
                return m_RendererDataList[m_DefaultRendererIndex];
            }
        }

        public ScriptableRenderer scriptableRenderer {
            get {
                m_Renderers[m_DefaultRendererIndex] = scriptableRendererData.InternalCreateRenderer();
                return m_Renderers[m_DefaultRendererIndex];
            }
        }

        // 
        public static MilesRenderingPipelineAsset Create(ScriptableRendererData rendererData = null) {
            var instance = CreateInstance<MilesRenderingPipelineAsset>();
            return instance;
        }

        protected override RenderPipeline CreatePipeline() {
            return new MilesRenderingPipeline(this);
        }
        public void OnAfterDeserialize() {
        }

        public void OnBeforeSerialize() {
        }

        // create MRP Assset menu
        [MenuItem("Assets/Create/MilesRendering/MilesRenderingPipeline/MilesRenderingPipeline Asset", priority = CoreUtils.Sections.section2 + CoreUtils.Priorities.assetsCreateRenderingMenuPriority + 1)]
        static void CreateMilesRenderingPipeline() {
            // enable name editing on the created asset
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, CreateInstance<CreateMilesRenderingPipelineAsset>(), "MilesRenderingPipeline Asset.asset", null, null);
        }

        // create renderer asset, this asset will be created while the mrp is created
        internal static ScriptableRendererData CreateRendererAsset(string path, MilesRendererType type, bool relativePath = true, string suffix = "Renderer") {
            ScriptableRendererData data = CreateRendererData(type);
            string dataPath;
            if (relativePath) {
                dataPath = $"{Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path))}_{suffix}{Path.GetExtension(path)}";
            } else {
                dataPath = path;
            }
            AssetDatabase.CreateAsset(data, dataPath);

            return data;
        }

        // get rendererData Instance
        static ScriptableRendererData CreateRendererData(MilesRendererType type) {
            switch (type) {
                case MilesRendererType.MilesRenderer:
                default: {
                        var rendererData = CreateInstance<MilesRendererData>();
                        return rendererData;
                    }
            }
        }

        // use a class to handle generation of mrp asset
        internal class CreateMilesRenderingPipelineAsset : EndNameEditAction {
            public override void Action(int instanceId, string pathName, string resourceFile) {

                AssetDatabase.CreateAsset(Create(CreateRendererAsset(pathName, MilesRendererType.MilesRenderer)), pathName);
            }
        }




    }
}
