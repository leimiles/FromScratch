using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
#endif

namespace UnityEngine.Funny.Rendering {
    /// <summary>
    /// 渲染接口类型，即 ScriptableRenderer
    /// </summary>
    public enum RendererType {
        Custom,
        FunnyRenderer,
        _2DRenderer

    }

    internal enum DefaultMaterialType {
        Unlit
    }

    // 渲染管线配置类，用于定义开启渲染管线的渲染流程，返回渲染管线实例，开启内置功能
    public partial class FunnyRenderPipelineAsset : RenderPipelineAsset, ISerializationCallbackReceiver {

        [SerializeField] internal ScriptableRendererData[] m_RendererDataList = new ScriptableRendererData[1];

        /// <summary>
        /// 创建材质时使用的默认 shader
        /// </summary>
        Shader m_DefaultShader;

        public override Shader defaultShader {
            get {
#if UNITY_EDITOR
                if (scriptableRendererData != null) {
                    Shader defaultShader = null;
                    if (defaultShader != null) {
                        return defaultShader;
                    }
                }

                if (m_DefaultShader == null) {
                    string path = AssetDatabase.GUIDToAssetPath(ShaderUtils.GetShaderGUID(ShaderPathID.Unlit));
                    m_DefaultShader = AssetDatabase.LoadAssetAtPath<Shader>(path);
                }
#endif
                if (m_DefaultShader == null) {
                    m_DefaultShader = Shader.Find(ShaderUtils.GetShaderPath(ShaderPathID.Unlit));
                }

                return m_DefaultShader;
            }
        }

        /// <summary>
        /// 创建对象时使用的默认材质
        /// </summary>
        public override Material defaultMaterial {
            get { return GetMaterial(DefaultMaterialType.Unlit); }
        }

        /// <summary>
        /// asset 当前包含的 renderer
        /// </summary>
        ScriptableRenderer[] m_Renderers = new ScriptableRenderer[1];

        /// <summary>
        /// 默认的 renderer 索引位置, 默认值为 0
        /// </summary>
        [SerializeField] internal int m_DefaultRendererIndex = 0;

        /// <summary>
        /// 当前渲染管线 asset 文件正在引用的 renderer
        /// </summary>
        public ScriptableRenderer scriptableRenderer {
            get {
                return m_Renderers[m_DefaultRendererIndex];
            }
        }

        /// <summary>
        /// 返回渲染管线的实例，渲染管线会按照该实例的 Render() 函数安排渲染流程
        /// </summary>
        protected override RenderPipeline CreatePipeline() {
            var pipeline = new FunnyRenderPipeline(this);
            CreateRenderers();
            return pipeline;
        }

#if UNITY_EDITOR
        /// <summary>
        /// 添加创建 asset 命令到菜单
        /// </summary>
        [MenuItem("Assets/Create/Funny/Rendering/FRP Asset", priority = CoreUtils.Sections.section1 + CoreUtils.Priorities.assetsCreateRenderingMenuPriority + 0)]
        static void CreateFunnyPipeline() {
            // 以指定名称创建 asset 后，允许重命名
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, CreateInstance<CreateFunnyPipelineAsset>(), "Funny Render Pipeline Asset.asset", null, null);
        }


        /// <summary>
        // 创建渲染管线 asset 文件，并允许重命名
        /// </summary>
        internal class CreateFunnyPipelineAsset : EndNameEditAction {
            // pathName 会传递当前 ScriptableObject 的路径，基于该路径设置 renderer asset 的位置
            public override void Action(int instanceId, string pathName, string resourceFile) {
                // 创建渲染管线 asset 文件，同时创建对应的 renderer asset 文件
                AssetDatabase.CreateAsset(Create(CreateRendererAsset(pathName, RendererType.FunnyRenderer)), pathName);
            }
        }

        /// <summary>
        /// 创建对应的 renderer asset 文件
        /// </summary>
        internal static FunnyRendererData CreateRendererAsset(string path, RendererType type, bool relativePath = true, string suffix = "Renderer") {
            FunnyRendererData FunnyRendererData = CreateRendererData(type);
            string dataPath;
            if (relativePath) {
                dataPath =
                $"{Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path))}_{suffix}{Path.GetExtension(path)}";
            } else {
                dataPath = path;
            }
            AssetDatabase.CreateAsset(FunnyRendererData, dataPath);
            return FunnyRendererData;
        }

#endif

        /// <summary>
        /// 创建渲染管线 asset 实例并返回，默认情况下，还会同时创建 renderer 的 asset 文件，配置给渲染管线后，返回其实例
        /// </summary>
        public static FunnyRenderPipelineAsset Create(ScriptableRendererData rendererData = null) {
            var instance = CreateInstance<FunnyRenderPipelineAsset>();
            if (rendererData != null) {
                instance.m_RendererDataList[0] = rendererData;
            } else {
                instance.m_RendererDataList[0] = CreateInstance<FunnyRendererData>();
            }
            return instance;
        }

        /// <summary>
        /// 创建渲染接口 renderer 实例
        /// </summary>
        void CreateRenderers() {
            if (m_Renderers == null) {
                m_Renderers = new ScriptableRenderer[1];
            }
            for (int i = 0; i < m_RendererDataList.Length; ++i) {
                if (m_RendererDataList[i] != null) {
                    m_Renderers[m_DefaultRendererIndex] = m_RendererDataList[i].InternalCreateRenderer();
                }
            }
        }


        /// <summary>
        /// 根据不同的 renderer 类型实现，创建不同的 renderer 实例并返回
        /// </summary>
        static FunnyRendererData CreateRendererData(RendererType type) {
            switch (type) {
                case RendererType.FunnyRenderer:
                default: {
                        var rendererData = CreateInstance<FunnyRendererData>();
                        return rendererData;
                    }
            }
        }

        internal ScriptableRendererData scriptableRendererData {
            get {
                if (m_RendererDataList[m_DefaultRendererIndex] == null) {
                    CreatePipeline();
                }
                return m_RendererDataList[m_DefaultRendererIndex];
            }
        }

        FunnyRenderPipelineEditorResources editorResources {
            get {
                return null;
            }

        }

        /// <summary>
        /// 检查渲染接口 renderer 的合法性
        /// </summary>
        internal bool ValidateRendererData(int index) {
            return false;
        }

        /// <summary>
        /// 返回指定渲染接口 renderer 的实例
        /// </summary>
        public ScriptableRenderer GetRenderer(int index) {
            return m_Renderers[index];
        }

        Material GetMaterial(DefaultMaterialType defaultMaterialType) {
#if UNITY_EDITOR
            if (scriptableRendererData == null) {
                return null;
            }

            // this will get null
            switch (defaultMaterialType) {
                case DefaultMaterialType.Unlit:
                    return null;
                default:
                    return null;
            }
#else
            return null;
#endif
        }

        public void OnAfterDeserialize() {
        }

        public void OnBeforeSerialize() {
        }

    }


}
