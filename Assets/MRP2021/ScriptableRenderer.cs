using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace MilesRenderingPipeline {
    public class ScriptableRenderer {
        internal static ScriptableRenderer current = null;
        static RenderTargetIdentifier[] m_ActiveColorAttachments = new RenderTargetIdentifier[] { 0, 0, 0, 0, 0, 0, 0, 0 };
        static RenderTargetIdentifier m_ActiveDepthAttachment;
        bool m_FirstTimeCameraColorTargetIsBound = true;
        bool m_FirstTimeCameraDepthTargetIsBound = true;
        RenderTargetIdentifier m_CameraColorTarget;
        RenderTargetIdentifier m_CameraDepthTarget;
        public ScriptableRenderer(ScriptableRendererData data) {

        }
        public void ClearTarget(CameraRenderType cameraRenderType) {
            m_ActiveColorAttachments[0] = BuiltinRenderTextureType.CameraTarget;
            for (int i = 1; i < m_ActiveColorAttachments.Length; ++i) {
                // 0 for none
                m_ActiveColorAttachments[i] = 0;
            }
            m_ActiveDepthAttachment = BuiltinRenderTextureType.CameraTarget;

            m_FirstTimeCameraColorTargetIsBound = cameraRenderType == CameraRenderType.Base;
            m_FirstTimeCameraDepthTargetIsBound = true;

            m_CameraColorTarget = BuiltinRenderTextureType.CameraTarget;
            m_CameraDepthTarget = BuiltinRenderTextureType.CameraTarget;

        }

        internal void OnPreCullRenderPasses(in CameraData cameraData) {

        }

        public virtual void SetupCullingParameters(ref ScriptableCullingParameters cullingParameters, ref CameraData cameraData) {
            // culling parameters must be set by child class
        }
    }
}
