using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEngine.Funny.Rendering {
    /// <summary>
    /// camera stack 用于渲染的设置数据集合，包括cullResualt, cameraData, shadowData, lightData, shadowData, postData 等其他设置信息
    /// </summary>
    public struct RenderingData {
        internal CommandBuffer commandBuffer;
        public CullingResults cullingResults;
        public CameraData cameraData;
    }

    /// <summary>
    /// 用于保存当前摄影机的设置信息，例如摄影机矩阵，投影矩阵，当前使用的 renderer，渲染目标等等
    /// </summary>
    public struct CameraData {
        Matrix4x4 m_ViewMatrix;
        Matrix4x4 m_ProjectionMatrix;

        // 即摄影机 component
        public Camera camera;

        // 摄影机当前使用的 renderer
        public ScriptableRenderer scriptableRenderer;

        // 当前摄影机的渲染类型，base 或者 overlay
        public CameraRenderType cameraRenderType;

        // 当前摄影机的视口类型
        public CameraType cameraType;

        // 当前摄影机的渲染目标，在为 null 的时候，摄影机的目标即窗口
        public RenderTexture cameraTargetTexture;

        // 用于描述摄影机渲染纹理的大小，格式等设置信息
        public RenderTextureDescriptor cameraRenderTextureDescriptor;

        // 是否是 scene view
        public bool isSceneViewCamera => cameraType == CameraType.SceneView;

        /// <summary>
        /// 用于设置摄影机矩阵
        /// </summary>
        internal void SetViewAndProjectionMatrix(Matrix4x4 viewMatrix, Matrix4x4 projectionMatrix) {
            m_ViewMatrix = viewMatrix;
            m_ProjectionMatrix = projectionMatrix;
        }

        /// <summary>
        /// 如果在非 OpenGL 设备上渲染 render texture，摄影机的 projection matrix 是反的，会影响诸如 blit 的结果
        /// </summary>
        public bool IsCameraProjectionMatrixFlipped() {
            if (ScriptableRenderer.currentRenderer != null) {
                var targetHandleId = ScriptableRenderer.currentRenderer.cameraColorTargetHandle?.nameID;
                bool renderingToBackBufferTarget = targetHandleId == BuiltinRenderTextureType.CameraTarget;
                if (ScriptableRenderer.currentRenderer.cameraColorTargetHandle == null) {
                    Debug.Log("todo not possible");
                }
                bool renderingToTexture = !renderingToBackBufferTarget || cameraTargetTexture != null;
                return SystemInfo.graphicsUVStartsAtTop;
            }
            return true;
        }
    }

}