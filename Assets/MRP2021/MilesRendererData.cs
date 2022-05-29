using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;
using UnityEngine.Rendering;

namespace MilesRenderingPipeline {
    [Serializable, ReloadGroup, ExcludeFromPreset]
    public class MilesRendererData : ScriptableRendererData, ISerializationCallbackReceiver {
        public void OnAfterDeserialize() {
        }
        public void OnBeforeSerialize() {

        }

    }
}
