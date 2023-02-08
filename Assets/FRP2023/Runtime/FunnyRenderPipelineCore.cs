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
        /*
        Matrix4x4 m_ViewMatrix;
        Matrix4x4 m_ProjectionMatrix;
        */

        // 即摄影机 component
        public Camera camera;
        // 摄影机当前使用的 renderer
        public ScriptableRenderer scriptableRenderer;


        // 当前摄影机的渲染类型，base 或者 overlay
        public CameraRenderType cameraRenderType;

        // 当前摄影机的渲染目标，在为 null 的时候，摄影机的目标即窗口
        public RenderTexture cameraTargetTexture;

        // 用于描述摄影机渲染纹理的大小，格式等设置信息
        public RenderTextureDescriptor cameraRenderTextureDescriptor;
    }

    /// <summary>
    /// 用于支持渲染管线类的辅助功能，partial
    /// </summary>
    public sealed partial class FunnyRenderPipeline {
        /// <summary>
        /// 以当前图形设置面板中的 asset 设置文件返回
        /// </summary>
        public static FunnyRenderPipelineAsset currentPipelineAsset {
            get => GraphicsSettings.currentRenderPipeline as FunnyRenderPipelineAsset;
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