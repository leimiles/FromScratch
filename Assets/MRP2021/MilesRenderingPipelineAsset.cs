using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine.Rendering;
using System.IO;

namespace MilesRenderingPipeline {
    // mrp Asset
    public class MilesRenderingPipelineAsset : RenderPipelineAsset, ISerializationCallbackReceiver {
        public enum MilesRendererType {
            MilesRenderer
        }
        Shader miles_DefaultShader;
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
        internal static MilesScriptableRenderingData CreateMilesRendererAsset(string path, MilesRendererType type, bool relativePath = true, string suffix = "Renderer") {
            // different renderer type for different renderer asset
            MilesScriptableRenderingData data = CreateRendererData(type);
            string dataPath;
            if (relativePath) {
                dataPath = $"{Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path))}_{suffix}{Path.GetExtension(path)}";

            } else {
                dataPath = path;
            }
            AssetDatabase.CreateAsset(data, dataPath);
            // ResourceReloader.ReloadAllNullIn ?
            return data;
        }

        // renderer asset is ditinguished here
        static MilesScriptableRenderingData CreateRendererData(MilesRendererType type) {
            switch (type) {
                case MilesRendererType.MilesRenderer:
                default: {
                        var rendererData = CreateInstance<MilesScriptableRenderingData>();
                        return rendererData;
                    }
            }
        }


        static MilesRenderingPipelineAsset Create(MilesScriptableRenderingData renderingData = null) {
            var instance = CreateInstance<MilesRenderingPipelineAsset>();
            return instance;

        }

        // use a class to handle generation of mrp asset
        internal class CreateMilesRenderingPipelineAsset : EndNameEditAction {
            public override void Action(int instanceId, string pathName, string resourceFile) {
                AssetDatabase.CreateAsset(Create(CreateMilesRendererAsset(pathName, MilesRendererType.MilesRenderer)), pathName);
            }

        }


    }
}
