using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEngine.Funny.Rendering {
    /// <summary>
    /// 用于正常渲染不透明序列的对象
    /// </summary>
    public class DrawObjectsPass : ScriptableRenderPass {

        List<ShaderTagId> m_ShaderTagIdLists = new List<ShaderTagId>();

        PassData m_PassData;
        bool m_IsOpaque;
        FilteringSettings m_FilteringSettings;
        RenderStateBlock m_RenderStateBlock;

        static ShaderTagId[] m_DefaultShaderTagIds = new ShaderTagId[] {
            new ShaderTagId("SRPDefaultUnlit")
        };

        public DrawObjectsPass(
            ShaderTagId[] shaderTagIds,
            bool isOpaque,
            RenderPassEvent renderPassEvent,
            RenderQueueRange renderQueueRange,
            LayerMask layerMask
            ) {
            m_PassData = new PassData();
            this.renderPassEvent = renderPassEvent;
            foreach (ShaderTagId shaderTagId in shaderTagIds) {
                m_ShaderTagIdLists.Add(shaderTagId);
            }
            m_IsOpaque = isOpaque;
            m_FilteringSettings = new FilteringSettings(renderQueueRange, layerMask);
            m_RenderStateBlock = new RenderStateBlock(RenderStateMask.Nothing);
        }

        public DrawObjectsPass(
            bool isOpaque,
            RenderPassEvent renderPassEvent,
            RenderQueueRange renderQueueRange,
            LayerMask layerMask
            ) : this(
            m_DefaultShaderTagIds,
            isOpaque,
            renderPassEvent,
            renderQueueRange,
            layerMask
            ) {

        }

        public override void Execute(ScriptableRenderContext renderContext, ref RenderingData renderingData) {
            m_PassData.m_ShaderTagIdList = m_ShaderTagIdLists;
            m_PassData.m_IsOpaque = m_IsOpaque;
            m_PassData.m_FilteringSettings = m_FilteringSettings;
            m_PassData.m_RenderStateBlock = m_RenderStateBlock;

            CameraSetup(renderingData.commandBuffer, m_PassData, ref renderingData);
            ExecutePass(renderContext, m_PassData, ref renderingData, renderingData.cameraData.IsCameraProjectionMatrixFlipped());

        }

        private static void ExecutePass(ScriptableRenderContext renderContext, PassData passData, ref RenderingData renderingData, bool yFlip) {

            var sortFlags = SortingCriteria.CommonOpaque;
            var filterSettings = passData.m_FilteringSettings;
            DrawingSettings drawSettings = RenderingUtils.CreateDrawingSettings(passData.m_ShaderTagIdList, ref renderingData, sortFlags);
            //Debug.Log(drawSettings == null ? "bad" : "good");
            renderContext.DrawRenderers(renderingData.cullingResults, ref drawSettings, ref filterSettings, ref passData.m_RenderStateBlock);
        }

        private static void CameraSetup(CommandBuffer commandBuffer, PassData passData, ref RenderingData renderingData) {

        }

        /// <summary>
        /// 用来记录不透明 pass 渲染所需要的资源信息
        /// </summary>
        private class PassData {
            internal bool m_IsOpaque;
            internal FilteringSettings m_FilteringSettings;
            internal List<ShaderTagId> m_ShaderTagIdList;
            internal RenderStateBlock m_RenderStateBlock;
        }
    }
}
