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

        static ShaderTagId[] m_DefaultShaderTagIds = new ShaderTagId[] {
            new ShaderTagId("SRPDefaultUnlit")
        };

        public DrawObjectsPass(
            ShaderTagId[] shaderTagIds,
            RenderPassEvent renderPassEvent
            ) {
            m_PassData = new PassData();
            this.renderPassEvent = renderPassEvent;
            foreach (ShaderTagId shaderTagId in shaderTagIds) {
                m_ShaderTagIdLists.Add(shaderTagId);
            }
        }

        public DrawObjectsPass(
            bool isOpaque,
            RenderPassEvent renderPassEvent
            ) : this(
            m_DefaultShaderTagIds,
            renderPassEvent
            ) {

        }

        public override void Execute(ScriptableRenderContext renderContext, ref RenderingData renderingData) {
            Debug.Log("opaque pass");
            m_PassData.m_ShaderTagIdList = m_ShaderTagIdLists;

            CameraSetup(renderingData.commandBuffer, m_PassData, ref renderingData);
            ExecutePass(renderContext, m_PassData, ref renderingData, renderingData.cameraData.IsCameraProjectionMatrixFlipped());

        }

        private static void ExecutePass(ScriptableRenderContext renderContext, PassData passData, ref RenderingData renderingData, bool yFlip) {
            //renderContext.DrawRenderers(renderingData.cullingResults, ref drawSettings, ref filterSettings, ref data.m_RenderStateBlock);
        }

        private static void CameraSetup(CommandBuffer commandBuffer, PassData passData, ref RenderingData renderingData) {

        }

        private class PassData {
            internal List<ShaderTagId> m_ShaderTagIdList;
        }
    }
}
