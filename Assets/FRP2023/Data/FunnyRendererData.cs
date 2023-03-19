using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Funny.Rendering {
    /// <summary>
    /// 用于实现 funny renderer 的所有资源数据
    /// </summary>
    public class FunnyRendererData : ScriptableRendererData, ISerializationCallbackReceiver {

        [SerializeField] LayerMask m_OpaqueLayerMask = -1;

        /// <summary>
        /// 设置不透明对象的过滤方式
        /// </summary>
        public LayerMask opaqueLayerMask {
            get => m_OpaqueLayerMask;
            set {
                SetDirty();
                m_OpaqueLayerMask = value;
            }

        }

        /// <summary>
        /// 实现 FunnyRenderer 实例
        /// </summary>
        protected override ScriptableRenderer Create() {
            return new FunnyRenderer(this);
        }

        public void OnAfterDeserialize() {
        }

        public void OnBeforeSerialize() {
        }
    }
}
