using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;

namespace UnityEngine.Miles.Rendering {
    // 渲染类型，即 ScriptableRenderer
    public enum RendererType {
        Custom,
        MilesRenderer,
        _2DRenderer

    }
    public partial class MilesRenderPipelineAsset : RenderPipelineAsset, ISerializationCallbackReceiver {
        public void OnAfterDeserialize() {
        }

        public void OnBeforeSerialize() {
        }

        // 返回渲染管线的实例
        protected override RenderPipeline CreatePipeline() {
            var pipeline = new MilesRenderPipeline();
            return pipeline;
        }

        // 添加创建 asset 命令到菜单
        [MenuItem("Assets/Create/Miles/Rendering/MRP Asset", priority = CoreUtils.Sections.section1 + CoreUtils.Priorities.assetsCreateRenderingMenuPriority + 0)]
        static void CreateMilesPipeline() {
            // 以指定名称创建 asset 后，允许重命名
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, CreateInstance<CreateMilesPipelineAsset>(), "Miles Render Pipeline Asset.asset", null, null);
        }

        // 创建渲染管线 asset 文件，并允许重命名
        internal class CreateMilesPipelineAsset : EndNameEditAction {
            // pathName 会传递当前 ScriptableObject 的路径，基于该路径设置 renderer asset 的位置
            public override void Action(int instanceId, string pathName, string resourceFile) {
                // 创建渲染管线 asset 文件，同时创建对应的 renderer asset 文件
                AssetDatabase.CreateAsset(Create(CreateRendererAsset(pathName, RendererType.MilesRenderer)), pathName);
            }
        }

        // 创建渲染管线 asset 实例并返回
        public static MilesRenderPipelineAsset Create(MilesRendererData rendererData = null) {
            var instance = CreateInstance<MilesRenderPipelineAsset>();
            return instance;
        }

        // 创建对应的 renderer asset 文件
        internal static MilesRendererData CreateRendererAsset(string path, RendererType type, bool relativePath = true, string suffix = "Renderer") {
            MilesRendererData milesRendererData = CreateRendererData(type);
            string dataPath;
            if (relativePath) {
                dataPath =
                $"{Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path))}_{suffix}{Path.GetExtension(path)}";
            } else {
                dataPath = path;
            }
            AssetDatabase.CreateAsset(milesRendererData, dataPath);
            return milesRendererData;
        }

        // 根据不同的渲染类型创建不同的 renderer
        static MilesRendererData CreateRendererData(RendererType type) {
            switch (type) {
                case RendererType.MilesRenderer:
                default: {
                        var rendererData = CreateInstance<MilesRendererData>();
                        return rendererData;
                    }
            }
        }

    }


}
