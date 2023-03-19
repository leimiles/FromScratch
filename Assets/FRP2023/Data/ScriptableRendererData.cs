using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Funny.Rendering {
    /// <summary>
    /// 用于保存 renderer 在执行时需要用到的资源
    /// </summary>
    public abstract class ScriptableRendererData : ScriptableObject {

        internal bool isInvalidated { get; set; }

        /// <summary>
        /// 使用 set dirty 让 render passes 重新创建
        /// </summary>
        public new void SetDirty() {
            isInvalidated = true;
        }

        /// <summary>
        /// 由子类 rendererData 实现具体 renderer 的实例
        /// </summary>
        protected abstract ScriptableRenderer Create();

        /// <summary>
        /// 可以通过 rendererData 创建 renderer 实例
        /// </summary>
        internal ScriptableRenderer InternalCreateRenderer() {
            return Create();
        }
#if UNITY_EDITOR
        internal virtual Material GetDefaultMaterial(DefaultMaterialType materialType) {
            return null;
        }

        internal virtual Shader GetDefaultShader() {
            return null;
        }
#endif

    }
}
