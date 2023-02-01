using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;

namespace UnityEngine.Miles.Rendering {
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

        // 创建渲染管线 asset 文件，并允许重命名
        internal class CreateMilesPipelineAsset : EndNameEditAction {
            public override void Action(int instanceId, string pathName, string resourceFile) {
                AssetDatabase.CreateAsset(Create(), pathName);
            }
        }

        // 添加创建 asset 命令到菜单
        [MenuItem("Assets/Create/Miles/Rendering/MRP Asset", priority = CoreUtils.Sections.section1 + CoreUtils.Priorities.assetsCreateRenderingMenuPriority + 0)]
        static void CreateMilesPipeline() {
            // 以指定名称创建 asset 后，允许重命名
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, CreateInstance<CreateMilesPipelineAsset>(), "Miles Render Pipeline Asset.asset", null, null);

        }

        // 创建渲染管线 asset 实例并返回
        public static MilesRenderPipelineAsset Create() {
            var instance = CreateInstance<MilesRenderPipelineAsset>();
            return instance;
        }

    }


}
