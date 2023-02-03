using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Miles.Rendering {
    /// <summary>
    /// MRP 对 scriptable renderer 的特殊实现
    /// </summary>
    public class MilesRendererData : ScriptableRendererData, ISerializationCallbackReceiver {

        /// <summary>
        /// 实现 MilesRenderer 实例
        /// </summary>
        protected override ScriptableRenderer Create() {
            return new MilesRenderer();
        }

        public void OnAfterDeserialize() {
        }

        public void OnBeforeSerialize() {
        }
    }
}
