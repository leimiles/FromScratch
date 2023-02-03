using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Miles.Rendering {
    /// <summary>
    /// 定义摄影机的渲染类型 base 或者 overlay
    /// </summary>
    public enum CameraRenderType {
        Base,
        Overlay
    }

    /// <summary>
    /// 一个挂给摄影机的 mono 为摄影机提供额外的属性与功能，是渲染管线对摄影机功能的扩展
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class MilesAdditionalCameraData : MonoBehaviour, ISerializationCallbackReceiver {
        [SerializeField] int m_RendererIndex = -1;
        // 摄影机的渲染类型，是 base 或者 overlay
        [SerializeField] CameraRenderType m_CameraRenderType = CameraRenderType.Base;
        public CameraRenderType cameraRenderType {
            get => m_CameraRenderType;
            set => m_CameraRenderType = value;
        }

        // 摄影机当前使用的渲染接口
        public ScriptableRenderer scriptableRenderer {
            get {
                if (MilesRenderPipeline.currentPipelineAsset is null) {
                    return null;
                }
                if (!MilesRenderPipeline.currentPipelineAsset.ValidateRendererData(m_RendererIndex)) {

                }
                return MilesRenderPipeline.currentPipelineAsset.GetRenderer(m_RendererIndex);
            }
        }

        public void OnAfterDeserialize() {
        }

        public void OnBeforeSerialize() {
        }
    }
}
