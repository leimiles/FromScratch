using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEngine.Miles.Rendering {
    /// <summary>
    /// 用于保存当前摄影机的设置信息
    /// </summary>
    public struct CameraData {
        /*
        Matrix4x4 m_ViewMatrix;
        Matrix4x4 m_ProjectionMatrix;
        */

        // 即摄影机 component
        public Camera camera;
        // 摄影机当前使用的 renderer
        public ScriptableRenderer scriptableRenderer;

        /*
        // 当前摄影机的渲染类型，base 或者 overlay
        public CameraRenderType cameraRenderType;

        // 当前摄影机的渲染目标，在为 null 的时候，摄影机的目标即窗口
        public RenderTexture cameraRenderTexture;

        // 用于描述摄影机渲染纹理的大小，格式等设置信息
        public RenderTextureDescriptor cameraRenderTextureDescriptor;
        */

    }
    /// <summary>
    /// 用于支持渲染管线类的辅助功能，partial
    /// </summary>
    public sealed partial class MilesRenderPipeline {
        /// <summary>
        /// 以当前图形设置面板中的 asset 设置文件返回
        /// </summary>
        public static MilesRenderPipelineAsset currentPipelineAsset {
            get => GraphicsSettings.currentRenderPipeline as MilesRenderPipelineAsset;
        }

        /// <summary>
        /// 用于判断是否是 game 窗口
        /// </summary>
        public static bool IsGameCamera(Camera camera) {
            if (camera == null) {
                throw new ArgumentNullException("camera null error");
            } else {
                return camera.cameraType == CameraType.Game;
            }
        }

    }
}