using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
#endif

namespace UnityEngine.Miles.Rendering {
    /// <summary>
    /// 渲染接口类型，即 ScriptableRenderer
    /// </summary>
    public enum RendererType {
        Custom,
        MilesRenderer,
        _2DRenderer

    }

    // 渲染管线配置类，用于定义开启渲染管线的渲染流程，返回渲染管线实例，开启内置功能
    public partial class MilesRenderPipelineAsset : RenderPipelineAsset, ISerializationCallbackReceiver {

        ScriptableRenderer[] m_Renderers = new ScriptableRenderer[1];

        /// <summary>
        /// 返回渲染管线的实例，渲染管线会按照该实例的 Render() 函数安排渲染流程
        /// </summary>
        protected override RenderPipeline CreatePipeline() {
            var pipeline = new MilesRenderPipeline(this);
            return pipeline;
        }

        /// <summary>
        /// 添加创建 asset 命令到菜单
        /// </summary>
        [MenuItem("Assets/Create/Miles/Rendering/MRP Asset", priority = CoreUtils.Sections.section1 + CoreUtils.Priorities.assetsCreateRenderingMenuPriority + 0)]
        static void CreateMilesPipeline() {
            // 以指定名称创建 asset 后，允许重命名
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, CreateInstance<CreateMilesPipelineAsset>(), "Miles Render Pipeline Asset.asset", null, null);
        }

        /// <summary>
        // 创建渲染管线 asset 文件，并允许重命名
        /// </summary>
        internal class CreateMilesPipelineAsset : EndNameEditAction {
            // pathName 会传递当前 ScriptableObject 的路径，基于该路径设置 renderer asset 的位置
            public override void Action(int instanceId, string pathName, string resourceFile) {
                // 创建渲染管线 asset 文件，同时创建对应的 renderer asset 文件
                AssetDatabase.CreateAsset(Create(CreateRendererAsset(pathName, RendererType.MilesRenderer)), pathName);
            }
        }

        /// <summary>
        /// 创建渲染管线 asset 实例并返回
        /// </summary>
        public static MilesRenderPipelineAsset Create(MilesRendererData rendererData = null) {
            var instance = CreateInstance<MilesRenderPipelineAsset>();
            return instance;
        }

        /// <summary>
        /// 创建对应的 renderer asset 文件
        /// </summary>
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

        /// <summary>
        /// 根据不同的渲染类型创建不同的 renderer 实例并返回
        /// </summary>
        static MilesRendererData CreateRendererData(RendererType type) {
            switch (type) {
                case RendererType.MilesRenderer:
                default: {
                        var rendererData = CreateInstance<MilesRendererData>();
                        return rendererData;
                    }
            }
        }

        /// <summary>
        /// 检查渲染接口 renderer 的合法性
        /// </summary>
        internal bool ValidateRendererData(int index) {
            return false;
        }

        /// <summary>
        /// 返回指定渲染接口的实例
        /// </summary>
        public ScriptableRenderer GetRenderer(int index) {
            return m_Renderers[index];
        }

        public void OnAfterDeserialize() {
        }

        public void OnBeforeSerialize() {
        }

    }


}
