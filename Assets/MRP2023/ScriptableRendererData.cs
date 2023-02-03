using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Miles.Rendering {
    /// <summary>
    /// 用于保存 renderer 在执行时需要用到的资源
    /// </summary>
    public abstract class ScriptableRendererData : ScriptableObject {
        /// <summary>
        /// 由子类 rendererData 实现具体 renderer 的实例
        /// </summary>
        protected abstract ScriptableRenderer Create();

        /// <summary>
        /// 可以通过 rendererData 创建 renderer 实例
        internal ScriptableRenderer InternalCreateRenderer() {
            return Create();
        }

    }
}
